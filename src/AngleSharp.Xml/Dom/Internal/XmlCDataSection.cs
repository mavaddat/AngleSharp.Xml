namespace AngleSharp.Xml.Dom
{
    using AngleSharp.Dom;
    using System;

    sealed class XmlCDataSection : Node, IXmlCDataSection
    {
        private String _data;

        internal XmlCDataSection(Document owner, String data)
            : base(owner, "#cdata-section", NodeType.CharacterData)
        {
            _data = data;
        }

        public String Data
        {
            get => _data;
            set => _data = value ?? String.Empty;
        }

        public Int32 Length => _data.Length;

        public IElement NextElementSibling
        {
            get
            {
                var sibling = ((INode)this).NextSibling;

                while (sibling != null && sibling.NodeType != NodeType.Element)
                {
                    sibling = sibling.NextSibling;
                }

                return sibling as IElement;
            }
        }

        public IElement PreviousElementSibling
        {
            get
            {
                var sibling = ((INode)this).PreviousSibling;

                while (sibling != null && sibling.NodeType != NodeType.Element)
                {
                    sibling = sibling.PreviousSibling;
                }

                return sibling as IElement;
            }
        }

        public override String NodeValue
        {
            get => _data;
            set => Data = value;
        }

        public override String TextContent
        {
            get => _data;
            set => Data = value;
        }

        public String Substring(Int32 offset, Int32 count) => _data.Substring(offset, Math.Min(count, _data.Length - offset));

        public void Append(String data) => _data += data;

        public void Insert(Int32 offset, String data) => _data = _data.Insert(offset, data);

        public void Delete(Int32 offset, Int32 count) => _data = _data.Remove(offset, Math.Min(count, _data.Length - offset));

        public void Replace(Int32 offset, Int32 count, String data)
        {
            Delete(offset, count);
            Insert(offset, data);
        }

        public void Before(params INode[] nodes)
        {
            var parent = ((INode)this).Parent;

            foreach (var node in nodes)
            {
                parent?.InsertBefore(node, this);
            }
        }

        public void After(params INode[] nodes)
        {
            var parent = ((INode)this).Parent;
            var reference = ((INode)this).NextSibling;

            foreach (var node in nodes)
            {
                parent?.InsertBefore(node, reference);
            }
        }

        public void Replace(params INode[] nodes)
        {
            Before(nodes);
            Remove();
        }

        public void Remove() => ((INode)this).Parent?.RemoveChild(this);

        public override Node Clone(Document owner, Boolean deep) => new XmlCDataSection(owner, _data);
    }
}