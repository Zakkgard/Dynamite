﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GSoft.Dynamite.Fields;
using GSoft.Dynamite.Logging;
using GSoft.Dynamite.ValueTypes;
using Microsoft.SharePoint;

namespace GSoft.Dynamite.ValueTypes.Writers
{
    /// <summary>
    /// Writes user values to SharePoint list items, field definition's DefaultValue
    /// and folder MetadataDefaults.
    /// </summary>
    public class UserValueWriter : BaseValueWriter<UserValue>
    {
        private ILogger log;

        /// <summary>
        /// Creates a new <see cref="UserValueWriter"/>
        /// </summary>
        /// <param name="log">Logging utility</param>
        public UserValueWriter(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Writes a user field value to a SPListItem
        /// </summary>
        /// <param name="item">The SharePoint List Item</param>
        /// <param name="fieldValueInfo">The field and value information</param>
        public override void WriteValueToListItem(SPListItem item, FieldValueInfo fieldValueInfo)
        {
            var userValue = fieldValueInfo.Value as UserValue;
            var newUserValue = userValue != null
                ? CreateSharePointUserValue(item.Web, userValue).ToString()
                : null;

            item[fieldValueInfo.FieldInfo.InternalName] = newUserValue;
        }

        /// <summary>
        /// Writes a User value as an SPField's default value.
        /// WARNING: This should only be used in scenarios where you have complete and exclusive programmatic
        /// access to item creation - because SharePoint has patchy support for this and your NewForm.aspx pages WILL break. 
        /// </summary>
        /// <param name="parentFieldCollection">The parent field collection within which we can find the specific field to update</param>
        /// <param name="fieldValueInfo">The field and value information</param>
        public override void WriteValueToFieldDefault(SPFieldCollection parentFieldCollection, FieldValueInfo fieldValueInfo)
        {
            var withDefaultVal = (FieldInfo<UserValue>)fieldValueInfo.FieldInfo;
            var field = parentFieldCollection[fieldValueInfo.FieldInfo.Id];

            if (withDefaultVal.DefaultValue != null)
            {
                field.DefaultValue = CreateSharePointUserValue(parentFieldCollection.Web, withDefaultVal.DefaultValue).ToString();

                this.log.Warn(
                    "Default value ({0}) set on field {1} with type User. SharePoint does not support default values on User fields. Only list items created programmatically will get the default value properly set. Setting a User-field default value may break your lists' NewForm.aspx people pickers. User folder metadata defaults for User default values instead.",
                    field.DefaultValue,
                    field.InternalName);
            }
        }

        /// <summary>
        /// Writes a field value as an SPFolder's default column value
        /// </summary>
        /// <param name="folder">The folder for which we wish to update a field's default value</param>
        /// <param name="fieldValueInfo">The field and value information</param>
        public override void WriteValuesToFolderDefault(SPFolder folder, FieldValueInfo fieldValueInfo)
        {
            throw new NotImplementedException();
        }

        private static SPFieldUserValue CreateSharePointUserValue(SPWeb web, UserValue userValue)
        {
            return new SPFieldUserValue(
                web,
                userValue.Id,
                HttpUtility.HtmlEncode(userValue.DisplayName));
        }
    }
}