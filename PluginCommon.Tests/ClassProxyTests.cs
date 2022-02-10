using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lotlab.PluginCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lotlab.PluginCommon.Tests
{
    [TestClass]
    public class ClassProxyTests
    {
        [TestMethod]
        public void GetTypeOfNameTest()
        {
            var type = ClassProxy.GetTypeOfName(typeof(TestClass).FullName);
            Assert.AreEqual(typeof(TestClass), type);
        }

        [TestMethod]
        public void CallMethodTest()
        {
            TestClass test = new TestClass();
            TestClassProxy proxy = new TestClassProxy(test);

            test.Value = 1;
            proxy.SetValue();
            Assert.AreEqual(0, test.Value);

            proxy.SetValue(1);
            Assert.AreEqual(1, test.Value);

            var ret = proxy.SetValueAdd(1, 2);
            Assert.AreEqual(3, ret);
            Assert.AreEqual(3, test.Value);

            ret = proxy.SetValueAdd(1, 2, 3);
            Assert.AreEqual(6, ret);
            Assert.AreEqual(6, test.Value);

            ret = proxy.SetValueAdd(1, 2, 3, 4);
            Assert.AreEqual(10, ret);
            Assert.AreEqual(10, test.Value);
        }

        [TestMethod]
        public void PropertyTest()
        {
            TestClass test = new TestClass();
            TestClassProxy proxy = new TestClassProxy(test);

            test.Value = 12;
            Assert.AreEqual(test.Value, proxy.Value);

            proxy.Value = 14;
            Assert.AreEqual(14, test.Value);
        }

        [TestMethod]
        public void FieldTest()
        {
            TestClass test = new TestClass();
            TestClassProxy proxy = new TestClassProxy(test);

            test.FieldValue = 12;
            Assert.AreEqual(test.FieldValue, proxy.FieldValue);

            proxy.FieldValue = 14;
            Assert.AreEqual(14, test.FieldValue);
        }

        [TestMethod]
        public void EventTest()
        {
            TestClass test = new TestClass();
            TestClassProxy proxy = new TestClassProxy(test);

            int val = 0;
            Action<int> act = (v) => { val = v; };
            proxy.Action += act;

            test.InvokeAction(1);
            Assert.AreEqual(1, val);

            proxy.Action -= act;

            test.InvokeAction(2);
            Assert.AreEqual(1, val);
        }
    }

    class TestClass
    {
        public int Value { get; set; }

        public event Action<int> Action;

        public int FieldValue = 0;

        public void SetValue()
        {
            Value = 0;
        }
        public void SetValue(int num)
        {
            Value = num;
        }

        public int SetValueAdd(int a, int b)
        {
            Value = a + b;
            return Value;
        }

        public int SetValueAdd(int a, int b, int c)
        {
            Value = a + b + c;
            return Value;
        }

        public int SetValueAdd(int a, int b, int c, int d)
        {
            Value = a + b + c + d;
            return Value;
        }

        public void InvokeAction(int val)
        {
            Action?.Invoke(val);
        }
    }

    class TestClassProxy : ClassProxy
    {
        public TestClassProxy(object instance) : base(typeof(TestClass), instance)
        {
        }

        public int Value
        {
            get => (int)PropertyGet();
            set => PropertySet(value);
        }

        public event Action<int> Action
        {
            add { EventAdd(value); }
            remove { EventRemove(value);  }
        }

        public int FieldValue
        {
            get => (int)FieldGet();
            set => FieldSet(value);
        }

        public void SetValue()
        {
            CallMethod();
        }
        public void SetValue(int num)
        {
            CallMethod(num);
        }

        public int SetValueAdd(int a, int b)
        {
            return (int)CallMethod(a, b);
        }

        public int SetValueAdd(int a, int b, int c)
        {
            return (int)CallMethod(a, b, c);
        }

        public int SetValueAdd(int a, int b, int c, int d)
        {
            return (int)CallMethod(a, b, c, d);
        }
    }
}