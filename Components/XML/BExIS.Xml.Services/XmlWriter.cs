﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

/// <summary>
///
/// </summary>        
namespace BExIS.Xml.Services
{
    /// <summary>
    /// 
    /// </summary>
    public enum XmlNodeMode
    {
        xPath,
        type
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>        
    public class XmlWriter
    {
        protected XmlNodeMode _mode;
        protected XDocument _tempXDoc;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected XElement CreateXElement(string name, XmlNodeType type)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                XElement element = new XElement(type.ToString());
                element.SetAttributeValue("name", name);
                element.SetAttributeValue("type", type.ToString());

                return element;
            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                XElement element = new XElement(name.Replace(" ", ""));
                element.SetAttributeValue("type", type.ToString());

                return element; 
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <returns></returns>
        protected bool Exist(string name)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                if (_tempXDoc.Root.Elements().Where(p => p.Attribute("name").Equals(name)).Count() > 0)
                    return true;
                else
                    return false;
            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                if (_tempXDoc.Root.Elements(name.Replace(" ","")).Count() > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected bool Exist(string name, int number)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                if (_tempXDoc.Root.Elements().Where(p => p.Attribute("name").Equals(name)
                    && p.Attribute("number").Equals(number.ToString())).Count() > 0)
                    return true;
                else
                    return false;
            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                if (_tempXDoc.Root.Elements(name.Replace(" ", "")).Where(p => p.Attribute("number") != null && p.Attribute("number").Value.Equals(number.ToString())).Count() > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected bool Exist(string name, XElement source)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                if (source.Elements().Where(p => p.Attribute("name").Equals(name)).Count() > 0)
                    return true;
                else
                    return false;
            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                if (source.Elements(name.Replace(" ", "")).Count() > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected bool Exist(string name, int number, XElement source)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                if (source.Elements().Where(p => p.Attribute("name").Equals(name)
                    && p.Attribute("number").Equals(number.ToString())).Count() > 0)
                    return true;
                else
                    return false;
            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                if (source.Elements(name.Replace(" ", "")).Where(p => p.Attribute("number") != null && p.Attribute("number").Value.Equals(number.ToString())).Count() > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <returns></returns>
        protected XElement Get(string name)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                return _tempXDoc.Root.Elements().Where(p => p.Attribute("name").Equals(name)).FirstOrDefault();

            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                return _tempXDoc.Root.Elements(name.Replace(" ", "")).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected XElement Get(string name, int number)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                return _tempXDoc.Root.Elements().Where(p => p.Attribute("name").Equals(name) && p.Attribute("number").Equals(number.ToString())).FirstOrDefault();

            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                return _tempXDoc.Root.Elements(name.Replace(" ", "")).Where(p => p.Attribute("number") != null && p.Attribute("number").Value.Equals(number.ToString())).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected XElement Get(string name, XElement source)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                return source.Elements().Where(p => p.Attribute("name").Equals(name)).FirstOrDefault();

            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                return source.Elements(name.Replace(" ", "")).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref=""/>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected XElement Get(string name, int number, XElement source)
        {
            if (_mode.Equals(XmlNodeMode.type))
            {
                return source.Elements().Where(p => p.Attribute("name").Equals(name) && p.Attribute("number").Equals(number.ToString())).FirstOrDefault();

            }

            if (_mode.Equals(XmlNodeMode.xPath))
            {
                return source.Elements(name.Replace(" ", "")).Where(p => p.Attribute("number") != null && p.Attribute("number").Value.Equals(number.ToString())).FirstOrDefault();
            }

            return null;
        }

        #region get list of xelement

            /// <summary>
            /// 
            /// </summary>
            /// <remarks></remarks>
            /// <seealso cref=""/>
            /// <param name="name"></param>
            /// <param name="source"></param>
            /// <returns></returns>
            protected List<XElement> GetChildren(string name, XElement source)
            {
                if (_mode.Equals(XmlNodeMode.type))
                {
                    return source.Elements().Where(p => p.Attribute("name").Equals(name)).ToList();

                }

                if (_mode.Equals(XmlNodeMode.xPath))
                {
                    return source.Elements(name.Replace(" ", "")).ToList();
                }

                return null;
            }
        #endregion

        #region static

                /// <summary>
                /// 
                /// </summary>
                /// <remarks></remarks>
                /// <seealso cref=""/>
                /// <param name="xDocument"></param>
                /// <returns></returns>
                public static XmlDocument ToXmlDocument(XDocument xDocument)
                {
                    var xmlDocument = new XmlDocument();
                    using (var xmlReader = xDocument.CreateReader())
                    {
                        xmlDocument.Load(xmlReader);
                    }
                    return xmlDocument;
                }

                /// <summary>
                /// 
                /// </summary>
                /// <remarks></remarks>
                /// <seealso cref=""/>
                /// <param name="xmlDocument"></param>
                /// <returns></returns>
                public static XDocument ToXDocument(XmlDocument xmlDocument)
                {
                    using (var nodeReader = new XmlNodeReader(xmlDocument))
                    {
                        nodeReader.MoveToContent();
                        return XDocument.Load(nodeReader);
                    }
                }
            #endregion

    }

}
