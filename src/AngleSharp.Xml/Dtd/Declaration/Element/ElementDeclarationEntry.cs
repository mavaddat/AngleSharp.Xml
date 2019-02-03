namespace AngleSharp.Xml.Dtd.Declaration
{
    using System;
    using System.Collections.Generic;

    abstract class ElementDeclarationEntry
    {
        #region Fields

        protected ElementContentType _type;

        private static ElementAnyDeclarationEntry _any;
        private static ElementEmptyDeclarationEntry _empty;

        #endregion

        #region Properties

        public ElementContentType Type
        {
            get { return _type; }
        }

        public static ElementAnyDeclarationEntry Any
        {
            get { return _any ?? (_any = new ElementAnyDeclarationEntry()); }
        }

        public static ElementEmptyDeclarationEntry Empty
        {
            get { return _empty ?? (_empty = new ElementEmptyDeclarationEntry()); }
        }

        #endregion

        #region Methods

        public abstract Boolean Check(NodeInspector inspector);

        #endregion
    }

    abstract class ElementQuantifiedDeclarationEntry : ElementDeclarationEntry
    {
        protected ElementQuantifier _quantifier;

        public ElementQuantifier Quantifier
        {
            get { return _quantifier; }
            set { _quantifier = value; }
        }
    }

    abstract class ElementChildrenDeclarationEntry : ElementQuantifiedDeclarationEntry
    {
        protected List<ElementQuantifiedDeclarationEntry> _children;

        public ElementChildrenDeclarationEntry()
        {
            _children = new List<ElementQuantifiedDeclarationEntry>();
            _type = ElementContentType.Children;
        }
    }
}
