namespace AngleSharp.Xml.Tests.Dom
{
    using AngleSharp.Dom;
    using AngleSharp.Xml.Dom;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class XmlDocumentFactories
    {
        [Test]
        public void DeclarationMetadataIsExposed()
        {
            var document = "<?xml version='1.0' encoding='ISO-8859-1' standalone='yes'?><root />".ToXmlDocument();

            Assert.AreEqual("1.0", document.XmlVersion);
            Assert.AreEqual("ISO-8859-1", document.XmlEncoding);
            Assert.IsTrue(document.XmlStandalone);
        }

        [Test]
        public void DeclarationMetadataUsesXmlDefaultsWhenOmitted()
        {
            var document = "<root />".ToXmlDocument();

            Assert.AreEqual("1.0", document.XmlVersion);
            Assert.IsNull(document.XmlEncoding);
            Assert.IsFalse(document.XmlStandalone);
        }

        [Test]
        public void ClonePreservesDeclarationMetadata()
        {
            var document = "<?xml version='1.0' encoding='utf-8' standalone='yes'?><root />".ToXmlDocument();
            var clone = (IXmlDocument)document.Clone();

            Assert.AreEqual(document.XmlVersion, clone.XmlVersion);
            Assert.AreEqual(document.XmlEncoding, clone.XmlEncoding);
            Assert.AreEqual(document.XmlStandalone, clone.XmlStandalone);
        }

        [Test]
        public void CreateCDataSectionPreservesLiteralMarkup()
        {
            var document = "<root />".ToXmlDocument();
            var section = document.CreateCDataSection("<message>&value</message>");

            document.DocumentElement.AppendChild(section);

            Assert.AreEqual(NodeType.CharacterData, section.NodeType);
            Assert.AreEqual("<root><![CDATA[<message>&value</message>]]></root>", document.ToXml());
        }

        [Test]
        public void ParsedCDataRemainsCData()
        {
            var document = "<root><![CDATA[<value>]]></root>".ToXmlDocument();

            Assert.IsInstanceOf<IXmlCDataSection>(document.DocumentElement.FirstChild);
            Assert.AreEqual("<value>", document.DocumentElement.TextContent);
            Assert.AreEqual("<root><![CDATA[<value>]]></root>", document.ToXml());
        }

        [Test]
        public void CreateCDataSectionRejectsClosingDelimiter()
        {
            var document = "<root />".ToXmlDocument();

            Assert.Throws<DomException>(() => document.CreateCDataSection("]]>") );
        }

        [Test]
        public void CreateEntityReferenceIsExplicitlyUnsupported()
        {
            var document = "<root />".ToXmlDocument();

            Assert.Throws<NotSupportedException>(() => document.CreateEntityReference("entity"));
        }
    }
}