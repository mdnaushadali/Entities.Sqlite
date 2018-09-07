﻿#region Namespaces

using Achilles.Entities.Linq;
using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;
using Entities.Sqlite.Tests.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests.Context
{
    public class PersistenceTest
    {
        private void AddProduct( TestDataContext context )
        {
            var supplier = new Supplier()
            {
                //Id is autogenerated
                Name = "TrinketsRUS"
            };

            context.Suppliers.Add( supplier );

            var product = new Product()
            {
                // Id = 1, Auto generated key
                Name = "Banana",
                Price = 4.75,
                SupplierId = supplier.Id
            };

            context.Products.Add( product );
        }

        [Fact]
        public void Database_can_insert()
        {           
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var supplier = new Supplier()
                {
                    //Id is autogenerated
                    Name = "TrinketsRUS"
                };

                context.Suppliers.Add( supplier );

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    SupplierId = supplier.Id
                };

                context.Products.Add( product );

                var productsCount = (from row in context.Products select row).Count();
                
                Assert.Equal( 1, productsCount );
            }
        }

        [Fact]
        public async Task Database_can_insert_async()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var supplier = new Supplier()
                {
                    //Id is autogenerated
                    Name = "TrinketsRUS"
                };

                context.Suppliers.Add( supplier );

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    SupplierId = supplier.Id
                };

                var result = await context.Products.AddAsync( product );

                Assert.Equal( 1, result );
            }
        }

        [Fact]
        public void Database_can_read_with_pk()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;
            
            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var product = context.Products.First( p => p.Id == 1 );
                var product2 = context.Products.First( p => p.Id == 2 );

                Assert.Equal( "Banana", product.Name );
                Assert.Equal( "Plum", product2.Name );
            }
        }

        [Fact]
        public void Database_Query_CanReadList()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                Assert.Equal( 2, context.Products.Count() );
                var products = context.Products.ToList();

                Assert.Equal( 2, products.Count() );
                Assert.Equal( "Banana", products[ 0 ].Name );
                Assert.Equal( "Plum", products[ 1 ].Name );
            }
        }

        [Fact]
        public async Task Database_Query_CanReadListAsync()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                context.Initialize();

                var products = await context.Products.ToListAsync();

                Assert.Equal( 2, products.Count() );
                Assert.Equal( "Banana", products[ 0 ].Name );
                Assert.Equal( "Plum", products[ 1 ].Name );
            }
        }

        [Fact]
        public void Database_can_update()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                AddProduct( context );
                
                var productsCount = context.Products.Select( p=> p.Id ).Count();
                Assert.Equal( 1, productsCount );

                var product = context.Products.First();

                product.Name = "Plum";
                context.Products.Update( product );

                var product2 = context.Products.First( p => p.Id == product.Id );

                Assert.Equal( "Plum", product2.Name );
            }
        }

        [Fact]
        public async Task Database_can_update_async()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                AddProduct( context );

                var productsCount = context.Products.Count();
                Assert.Equal( 1, productsCount );

                var product = context.Products.First();

                product.Name = "Plum";
                var result = await context.Products.UpdateAsync( product );

                var product2 = context.Products.First( p => p.Id == product.Id );

                Assert.Equal( "Plum", product2.Name );
            }
        }

        [Fact]
        public void Database_can_delete_by_pk()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var supplier = new Supplier()
                {
                    //Id is autogenerated
                    Name = "TrinketsRUS"
                };

                context.Suppliers.Add( supplier );

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    SupplierId = supplier.Id
                    
                };

                context.Products.Add( product );
                var productsCount = context.Products.Count();

                Assert.Equal( 1, productsCount );

                context.Products.Delete( product );

                Assert.Equal( 0, context.Products.Select( p => p.Id ).Count() );
           }
        }

        [Fact]
        public async Task Database_can_delete_by_pk_async()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DataContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDataContext( options ) )
            {
                var createResult = context.Database.Creator.CreateIfNotExists();

                var supplier = new Supplier()
                {
                    //Id is autogenerated
                    Name = "TrinketsRUS"
                };

                context.Suppliers.Add( supplier );

                var product = new Product()
                {
                    // Id = 1, Auto generated key
                    Name = "Banana",
                    Price = 4.75,
                    SupplierId = supplier.Id
                };

                context.Products.Add( product );
                var productsCount = context.Products.Count();

                Assert.Equal( 1, productsCount );

                await context.Products.DeleteAsync( product );

                Assert.Equal( 0, context.Products.Select( p => p.Id ).Count() );
            }
        }
    }
}