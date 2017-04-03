using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests.General
{
    /// <summary>
    /// Summary description for Implicit_Explicit_ByRef_ByVal_Tests
    /// </summary>
    [TestClass]
    public class Implicit_Explicit_ByRef_ByVal_Tests
    {
        class ClassParameter { public string Print; }

        struct StructParameter { public string Print; }

        void Amend_Implicit<TEntity>(TEntity entity)
        {
            if (entity is ClassParameter)
                (entity as ClassParameter).Print += " World";
            else if (entity is StructParameter)
            {
                StructParameter parameter = ((StructParameter)(object)entity);

                parameter.Print += " World";
            }
        }

        void Amend_Explicit_By_Ref<TEntity>(ref TEntity entity)
        {
            if (entity is ClassParameter)
                (entity as ClassParameter).Print += " World";
            else if (entity is StructParameter)
            {
                StructParameter parameter = ((StructParameter)(object)entity);

                parameter.Print += " World";
            }
        }

        [TestMethod]
        public void TestAmend_ImplicitRef()
        {
            ClassParameter p1 = new ClassParameter { Print = "Hello" };
            StructParameter p2 = new StructParameter { Print = "Hello" };

            Amend_Implicit(p1);
            Amend_Implicit(p2);

            Assert.AreEqual("Hello World", p1.Print);
            Assert.AreEqual("Hello", p2.Print, "Structs are always passed-by-value");
        }

        [TestMethod]
        public void TestAmend_ExplicitRef()
        {
            ClassParameter p1 = new ClassParameter { Print = "Hello" };
            StructParameter p2 = new StructParameter { Print = "Hello" };

            Amend_Explicit_By_Ref(ref p1);
            Amend_Explicit_By_Ref(ref p2);

            Assert.AreEqual("Hello World", p1.Print);
            Assert.AreEqual("Hello", p2.Print, "Structs are always passed-by-value");
        }
    }
}
