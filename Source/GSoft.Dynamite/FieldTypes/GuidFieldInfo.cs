﻿using System;
using System.Xml.Linq;

namespace GSoft.Dynamite.Definitions
{
    /// <summary>
    /// Definition of a GUID field
    /// </summary>
    public class GuidFieldInfo : FieldInfo<Guid>
    {
        /// <summary>
        /// Initializes a new NoteFieldInfo
        /// </summary>
        /// <param name="internalName">The internal name of the field</param>
        /// <param name="id">The field identifier</param>
        public GuidFieldInfo(string internalName, Guid id)
            : base(internalName, id, "Guid")
        {
        }

        /// <summary>
        /// The XML schema of a GUID field as XElement
        /// </summary>
        /// <returns>The XML schema of a GUID field as XElement</returns>
        public override XElement Schema
        {
            get
            {
                return new XElement(
                    "Field",
                    new XAttribute("Name", this.InternalName),
                    new XAttribute("Type", this.Type),
                    new XAttribute("ID", "{" + this.Id + "}"),
                    new XAttribute("StaticName", this.InternalName),
                    new XAttribute("DisplayName", this.DisplayName),
                    new XAttribute("Description", this.Description),
                    new XAttribute("Group", this.Group));
            }
        }
    }
}
