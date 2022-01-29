using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Cfg.ConfigurationSchema;
using ProductShop.Data;
using ProductShop.Datasets.Dtos.InputModels;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            string path = @"C:\Users\Ivan\EFCore\EFCore\10.JSON\ProductShop\ProductShop\Datasets\users.json";
            string pathProducts = @"C:\Users\Ivan\EFCore\EFCore\10.JSON\ProductShop\ProductShop\Datasets\products.json";
            string pathCategories = @"C:\Users\Ivan\EFCore\EFCore\10.JSON\ProductShop\ProductShop\Datasets\categories.json";
            string pathCategoriesProduct = @"C:\Users\Ivan\EFCore\EFCore\10.JSON\ProductShop\ProductShop\Datasets\categories-products.json";
            var jsonStringReader = File.ReadAllText(path);
            var jsonStringReaderProducts = File.ReadAllText(pathProducts);
            var jsonStringReaderCategory = File.ReadAllText(pathCategories);
            var jsonStringReaderCategoryProduct = File.ReadAllText(pathCategoriesProduct);
            string result = ImportUsers(context, jsonStringReader);
            string resultProducts = ImportProducts(context, jsonStringReaderProducts);
            string categoriesResult = ImportCategories(context, jsonStringReaderCategory);
            string categoryProductResult = ImportCategoryProducts(context, jsonStringReaderCategoryProduct);
            GetProductsInRange(context);
            GetSoldProducts(context);
            Console.WriteLine(result);
            Console.WriteLine(resultProducts);
            Console.WriteLine(categoriesResult);
            Console.WriteLine(categoryProductResult);

        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);

            MappConfiguration();

            var mappedUsers = mapper.Map<IEnumerable<User>>(users);

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();
            return $"Successfully imported {mappedUsers.Count()}";
        }

        private static void MappConfiguration()
        {
            MapperConfiguration mappingConfiguration = new MapperConfiguration(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });
            mapper = new Mapper(mappingConfiguration);
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<ProductInputDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductInputDto>>(inputJson);
            MappConfiguration();
            var mapProduct = mapper.Map<IEnumerable<Product>>(products);
            context.Products.AddRange(mapProduct);
            context.SaveChanges();
            return $"Successfully imported {mapProduct.Count()}"; ;
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryInputDto> categories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputDto>>(inputJson)
                .Where(x => !string.IsNullOrEmpty(x.Name));
            MappConfiguration();
            var mappCategories = mapper.Map<IEnumerable<Category>>(categories);
            context.Categories.AddRange(mappCategories);
            context.SaveChanges();
            return $"Successfully imported {mappCategories.Count()}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryProductDto> categoryProducts = JsonConvert.DeserializeObject<IEnumerable<CategoryProductDto>>(inputJson);
            MappConfiguration();
            var mapCategoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoryProducts);
            context.CategoryProducts.AddRange(mapCategoryProducts);
            context.SaveChanges();
            return $"Successfully imported {mapCategoryProducts.Count()}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = $"{x.Seller.FirstName} {x.Seller.LastName}"

                })
                .ToList();
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };
            string productsAsJson = JsonConvert.SerializeObject(products, jsonSettings);
            return productsAsJson;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
               .Where(x => x.ProductsSold.Any(y => y.Buyer != null) )
               .OrderBy(x => x.LastName)
               .ThenBy(x => x.FirstName)
               .Select(x => new
               {
                   firstName = x.FirstName,
                   lastName = x.LastName,
                   soldProducts = x.ProductsSold.Select(s => new
                   {
                       name = s.Name,
                       price = s.Price,
                       buyerFirstName = s.Buyer.FirstName,
                       buyerLastName = s.Buyer.LastName
                   })
                   .ToList()
               })
               .ToList();

            string usersAsJson = JsonConvert.SerializeObject(users);
            return usersAsJson;
        }
    }
}