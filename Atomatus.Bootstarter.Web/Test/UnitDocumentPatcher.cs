using System;
using System.Collections;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Com.Atomatus.Bootstarter.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitDocumentPatcher
    {
        [TestMethod]
        public void TestDocumentPatcher_Successfully()
        {
            ProxyGenerator proxyGenerator = new();
            OrderDTO orderDto = proxyGenerator.CreateClassProxy<OrderDTO>(new LazyLoadingInterceptor());
            orderDto.Id = 10;

            Order order = new();
            orderDto.ApplyTo(order);

            Assert.AreEqual(order.Id, orderDto.Id);
            Assert.IsNotNull(order.Items);
            Assert.IsNotNull(orderDto.Items);

            int i = 0;
            foreach (var item in orderDto.Items)
            {
                Assert.AreEqual(order.Items[i].Id, item.Id);
                Assert.AreEqual(order.Items[i++].Name, item.Name);
            }
        }
    }

    public class LazyLoadingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.StartsWith("get_") && invocation.Method.ReturnType.IsGenericType)
            {
                invocation.ReturnValue = new LazyEnumerable<OrderItemDTO>(() =>
                {
                    return new[]
                    {
                            new OrderItemDTO() { Id = 1, Name = "Item 1" },
                            new OrderItemDTO() { Id = 2, Name = "Item 2" },
                            new OrderItemDTO() { Id = 3, Name = "Item 3" }
                        };
                });
            }
        }
    }

    public class LazyEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerable<T>> func;

        public LazyEnumerable(Func<IEnumerable<T>> func)
        {
            this.func = func;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return func.Invoke().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return func.Invoke().GetEnumerator();
        }
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class OrderItemDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public List<OrderItem> Items { get; set; }
    }

    public class OrderDTO : IDocumentPatcher
    {
        public int Id { get; set; }

        public virtual LazyEnumerable<OrderItemDTO> Items { get; set; }
    }
}
