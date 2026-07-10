using Elastic.Clients.Elasticsearch;
using Shop.Domain.Dtoes.Product;
using Shop.Domain.Dtoes;
using Shop.Infrastructure.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace Shop.Infrastructure.ElasticSearch
{
    public class ElasticProductService : IElasticProductService
    {
        private readonly IElasticSearchService _elastic;
        private readonly ElasticsearchClient _client;

        public ElasticProductService(IElasticSearchService elastic, ElasticsearchClient client)
        {
            _elastic = elastic;
            _client = client;
        }

        public void IndexProduct(ProductDto product)
        {
            var doc = new ProductDocument
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive
            };

            _elastic.Index(doc, ElasticIndexes.Products);
        }

        public void UpdateProduct(ProductDto product)
        {
            var doc = new ProductDocument
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive
            };

            _elastic.Update(doc, ElasticIndexes.Products);
        }

        public void DeleteProduct(int productId)
        {
            _elastic.Delete(ElasticIndexes.Products, productId.ToString());
        }
        private static readonly Dictionary<string, Expression<Func<ProductDocument, object?>>> SortFields =
            new()
            {
                { "Id", x => x.Id },
                { "Price", x => x.Price },
                { "CategoryId", x => x.CategoryId },
                { "IsActive", x => x.IsActive },
                { "ProductName", x => x.ProductName.Suffix("keyword") }
            };
        public List<ProductDto> SearchProducts(FilteringDto filter)
        {
            SearchResponse<ProductDocument> response;

            response = _client.Search<ProductDocument>(s => s
                .Indices(ElasticIndexes.Products)
                .From((filter.PageId - 1) * filter.PageSize)//hamoon skip
                .Size(filter.PageSize)//hamoon take
                .Query(q => q
                    .Bool(b =>
                    {
                        if (!string.IsNullOrWhiteSpace(filter.Search))
                        {
                            b.Must(m => m//hatman ino must bezar chon search value asli ast baqie Filter kafie
                                         //roye 2 ta field search kardim age yki bood mishod Match
                                .MultiMatch(mm => mm
                                    .Query(filter.Search)
                                    .Fields(f => f.ProductName, f => f.Description)
                                    //.Type(TextQueryType.CrossFields)//in mige 2 ta field bala ra mesl yek field vahed dar nazar begir
                                    .Operator(Operator.And)//inja yani 2ta field bala and beshan bja OR
                                    .Fuzziness("AUTO")//baraye eshtebah typi ham miare
                                                      //Type(TextQueryType.CrossFields) ba Fuzziness baham nemishe faqat yki
                                )
                            );
                        }

                        if (filter.CategoryId.HasValue)
                        {
                            b.Filter(f => f
                                .Term(t => t
                                    .Field(x => x.CategoryId)
                                    .Value(filter.CategoryId.Value)
                                )
                            );
                        }

                        if (filter.MinPrice.HasValue || filter.MaxPrice.HasValue)
                        {
                            b.Filter(f => f
                                .Range(r => r
                                    .Number(n => n
                                        .Field(x => x.Price)
                                        .Gte(filter.MinPrice.HasValue ? (double)filter.MinPrice.Value : null)
                                        .Lte(filter.MaxPrice.HasValue ? (double)filter.MaxPrice.Value : null)
                                    )
                                )
                            );
                        }
                        //inja bayad dar filter ye bool IsActive ezafe koni
                        //if (filter.IsActive.HasValue)
                        //{
                        //    b.Filter(f => f
                        //        .Term(t => t
                        //            .Field(x => x.IsActive)
                        //            .Value(filter.IsActive.Value)
                        //        )
                        //    );
                        //}
                        var sortBy = filter.SortBy ?? "Id";
                        if (SortFields.TryGetValue(sortBy, out var field))//bayad Dic migereftam ta betoone hamoon Reflection Repository piade sazi kone
                        {
                            s.Sort(sort => sort
                                .Field(f => f
                                    .Field(field)
                                    .Order(filter.SortOrder == "desc"
                                        ? SortOrder.Desc
                                        : SortOrder.Asc)
                                ));
                        }
                        else
                        {
                            s.Sort(sort => sort
                                .Field(f => f
                                    .Field(x => x.Id)
                                    .Order(SortOrder.Asc)
                                ));
                        }
                    })
                )
            );

            //in vase didane detail o khata hastesh
            var debug = response.DebugInformation;

            if (!response.IsValidResponse)
                throw new Exception(response.DebugInformation);

            var products = response.Documents
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    IsActive = x.IsActive
                })
                .ToList();

            return products;
        }
    }
}
