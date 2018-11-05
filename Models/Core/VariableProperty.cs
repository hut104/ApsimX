﻿// -----------------------------------------------------------------------
// <copyright file="VariableProperty.cs" company="APSIM Initiative">
// Copyright (c) APSIM Initiative
// </copyright>
//-----------------------------------------------------------------------
namespace Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Models.Soils;
    using System.Globalization;
    using APSIM.Shared.Utilities;
    using System.Collections;

    /// <summary>
    /// Encapsulates a discovered property of a model. Provides properties for
    /// returning information about the property. 
    /// </summary>
    [Serializable]
    public class VariableProperty : IVariable
    {
        /// <summary>
        /// Gets or sets the PropertyInfo for this property.
        /// </summary>
        private PropertyInfo property;

        /// <summary>
        /// An optional lower bound array index.
        /// </summary>
        private int lowerArraySpecifier;

        /// <summary>
        /// An optional upper bound array index.
        /// </summary>
        private int upperArraySpecifier;

        /// <summary>The name of the property to call on each array element.</summary>
        private string elementPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableProperty" /> class.
        /// </summary>
        /// <param name="model">The underlying model for the property</param>
        /// <param name="property">The PropertyInfo for this property</param>
        /// <param name="arraySpecifier">An optional array specification e.g. 1:3</param>
        public VariableProperty(object model, PropertyInfo property, string arraySpecifier = null)
        {
            if (model == null || property == null)
            {
                throw new ApsimXException(null, "Cannot create an instance of class VariableProperty with a null model or propertyInfo");
            }

            this.Object = model;
            this.property = property;
            if (arraySpecifier != null)
            {
                // Can be either a number or a range e.g. 1:3
                int posColon = arraySpecifier.IndexOf(':');
                if (posColon == -1)
                {
                    this.lowerArraySpecifier = Convert.ToInt32(arraySpecifier);
                    this.upperArraySpecifier = this.lowerArraySpecifier;
                }
                else
                {
                    this.lowerArraySpecifier = Convert.ToInt32(arraySpecifier.Substring(0, posColon));
                    this.upperArraySpecifier = Convert.ToInt32(arraySpecifier.Substring(posColon + 1));
                }
            }
            else
            {
                this.lowerArraySpecifier = 0;
                this.upperArraySpecifier = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableProperty" /> class.
        /// </summary>
        /// <param name="model">The underlying model for the property</param>
        /// <param name="elementPropertyName">The name of the property to call on each array element.</param>
        public VariableProperty(object model, string elementPropertyName)
        {
            this.Object = model;
            this.elementPropertyName = elementPropertyName;
        }

        /// <summary>
        /// Gets or sets the underlying model that this property belongs to.
        /// </summary>
        public override object Object { get; set; }

        /// <summary>
        /// Return the name of the property.
        /// </summary>
        public override string Name
        {
            get
            {
                return this.property.Name;
            }
        }

        /// <summary>
        /// Gets the description of the property
        /// </summary>
        public override string Description
        {
            get
            {
                DescriptionAttribute descriptionAttribute = ReflectionUtilities.GetAttribute(this.property, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (descriptionAttribute == null)
                {
                    return null;
                }

                if (this.Object is ISoilCrop)
                {
                    string cropName = (this.Object as ISoilCrop).Name;
                    if (cropName.EndsWith("Soil"))
                        cropName = cropName.Replace("Soil", "");
                    return cropName + " " + descriptionAttribute.ToString();
                }

                return descriptionAttribute.ToString();
            }
        }

        /// <summary>
        /// Gets the text to use as a label for the property.
        /// This is derived from the BriefLabel attribute or,
        /// if that does not exist, from the Description attribute
        /// </summary>
        public override string Caption
        {
            get
            {
                CaptionAttribute labelAttribute = ReflectionUtilities.GetAttribute(this.property, typeof(CaptionAttribute), false) as CaptionAttribute;
                if (labelAttribute == null)
                {
                    return Description;
                }
                else
                {
                    return labelAttribute.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the units of the property
        /// </summary>
        public override string Units
        {
            get
            {
                string unitString = null;
                UnitsAttribute unitsAttribute = ReflectionUtilities.GetAttribute(this.property, typeof(UnitsAttribute), false) as UnitsAttribute;
                PropertyInfo unitsInfo = this.Object.GetType().GetProperty(this.property.Name + "Units");
                if (unitsAttribute != null)
                {
                    unitString = unitsAttribute.ToString();
                }
                else if (unitsInfo != null)
                {
                    object val = unitsInfo.GetValue(this.Object, null);
                    unitString = val.ToString();
                }
                return unitString;
            }
            set
            {
                PropertyInfo unitsInfo = this.Object.GetType().GetProperty(this.property.Name + "Units");
                MethodInfo unitsSet = this.Object.GetType().GetMethod(this.property.Name + "UnitsSet");
                if (unitsSet != null)
                {
                    unitsSet.Invoke(this.Object, new object[] { Enum.Parse(unitsInfo.PropertyType, value) });
                }
                else if (unitsInfo != null)
                {
                    unitsInfo.SetValue(this.Object, Enum.Parse(unitsInfo.PropertyType, value), null);
                }
            }
        }

        /// <summary>
        /// Gets the units of the property as formmatted for display (in parentheses) or null if not found.
        /// </summary>
        public override string UnitsLabel
        {
            get
            {
                // Get units from property
                string unitString = null;
                UnitsAttribute unitsAttribute = ReflectionUtilities.GetAttribute(this.property, typeof(UnitsAttribute), false) as UnitsAttribute;
                PropertyInfo unitsInfo = this.Object.GetType().GetProperty(this.property.Name + "Units");
                if (unitsAttribute != null)
                {
                    unitString = unitsAttribute.ToString();
                }
                else if (unitsInfo != null)
                {
                    object val = unitsInfo.GetValue(this.Object, null);
                    if (unitsInfo != null && unitsInfo.PropertyType.BaseType == typeof(Enum))
                        unitString = GetEnumDescription(val as Enum);
                    else
                        unitString = val.ToString();
                }
                if (unitString != null)
                    return "(" + unitString + ")";
                else
                    return null;
            }
        }

        /// <summary>
        /// Looks for a description string associated with an enumerated value
        /// Adapted from http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].ToString();
            else
                return value.ToString();
        }

        /// <summary>
        /// Simple structure to hold both a name and an associated label
        /// </summary>
        public struct NameLabelPair
        {
            /// <summary>
            /// Name of an object
            /// </summary>
            public string Name;
            /// <summary>
            /// Display label for the object
            /// </summary>
            public string Label;
            /// <summary>
            /// Constructs a NameLabelPair
            /// </summary>
            /// <param name="name">Name of the object</param>
            /// <param name="label">Display label for the object</param>
            public NameLabelPair(string name, string label = null)
            {
                Name = name;
                if (String.IsNullOrEmpty(label))
                    Label = name;
                else
                    Label = label;
            }
        }

        /// <summary>
        /// Gets a list of allowable units
        /// The list contains both the actual name and a display name for each entry
        /// </summary>
        public NameLabelPair[] AllowableUnits
        {
            get
            {
                PropertyInfo unitsInfo = this.Object.GetType().GetProperty(this.property.Name + "Units");
                if (unitsInfo != null && unitsInfo.PropertyType.BaseType == typeof(Enum))
                {
                    Array enumValArray = Enum.GetValues(unitsInfo.PropertyType);
                    List<NameLabelPair> enumValList = new List<NameLabelPair>(enumValArray.Length);

                    foreach (int val in enumValArray)
                    {
                        object parsedEnum = Enum.Parse(unitsInfo.PropertyType, val.ToString());
                        enumValList.Add(new NameLabelPair(parsedEnum.ToString(), GetEnumDescription((Enum)parsedEnum)));
                    }
                    return enumValList.ToArray();
                }
                else
                    return new NameLabelPair[0];
            }
        }

        /// <summary>
        /// Gets a value indicating whether the property is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (!this.property.CanWrite)
                {
                    return true;
                }

                if (this.Metadata.Contains("Estimated") || this.Metadata.Contains("Calculated"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the metadata for each layer. Returns new string[0] if none available.
        /// </summary>
        public string[] Metadata
        {
            get
            {
                PropertyInfo metadataInfo = this.Object.GetType().GetProperty(this.property.Name + "Metadata");
                if (metadataInfo != null)
                {
                    string[] metadata = metadataInfo.GetValue(this.Object, null) as string[];
                    if (metadata != null)
                    {
                        return metadata;
                    }
                }

                return new string[0];
            }
        }

        /// <summary>
        /// Gets the data type of the property
        /// </summary>
        public override Type DataType
        {
            get
            {
                return this.property.PropertyType;
            }
        }

        /// <summary>
        /// Gets the values of the property
        /// </summary>
        public override object Value
        {
            get
            {
                if (elementPropertyName != null)
                    return ProcessPropertyOfArrayElement();

                object obj = null;
                try
                {
                    obj = this.property.GetValue(this.Object, null);
                }
                catch (Exception err)
                {
                    throw err.InnerException;
                }
                if (this.lowerArraySpecifier != 0 && obj != null && obj is IList)
                {
                    IList array = obj as IList;
                    if (array.Count == 0)
                        return null;

                    // Get the type of the items in the array.
                    Type elementType;
                    if (array.GetType().HasElementType)
                        elementType = array.GetType().GetElementType();
                    else
                    {
                        Type[] genericArguments = array.GetType().GetGenericArguments();
                        if (genericArguments.Length > 0)
                            elementType = genericArguments[0];
                        else
                            throw new Exception("Unknown type of array");
                    }

                    int numElements = this.upperArraySpecifier - this.lowerArraySpecifier + 1;
                    Array values = Array.CreateInstance(elementType, numElements);
                    for (int i = this.lowerArraySpecifier; i <= this.upperArraySpecifier; i++)
                    {
                        int index = i - this.lowerArraySpecifier;
                        if (i < 1 || i > array.Count)
                        {
                            throw new Exception("Array index out of bounds while getting variable: " + this.Name);
                        }

                        values.SetValue(array[i - 1], index);
                    }
                    if (values.Length == 1)
                    {
                        return values.GetValue(0);
                    }

                    return values;
                }

                return obj;
            }

            set
            {
                if (value is string)
                {
                    this.SetFromString(value as string);
                }
                else
                {
                    if (this.lowerArraySpecifier != 0)
                    {
                        object obj = null;
                        try
                        {
                            obj = this.property.GetValue(this.Object, null);
                        }
                        catch (Exception err)
                        {
                            throw err.InnerException;
                        }
                        IList array = obj as IList;

                        if (obj != null && obj is IList)
                        {
                            array[lowerArraySpecifier - 1] = value;
                            this.property.SetValue(this.Object, obj, null);
                        }
                    }
                    else
                        this.property.SetValue(this.Object, value, null);
                }
            }
        }

        /// <summary>
        /// Special case where trying to get a property of an array(IList). In this case
        /// we want to return the property value for all items in the array.
        /// e.g. Maize.Root.Zones.WaterUptake
        /// Zones is a List of ZoneState objects.
        /// </summary>
        private object ProcessPropertyOfArrayElement()
        {
            IList list = Object as IList;

            // Get the type of the items in the array.
            Type elementType;
            if (list.GetType().HasElementType)
                elementType = list.GetType().GetElementType();
            else
            {
                Type[] genericArguments = list.GetType().GetGenericArguments();
                if (genericArguments.Length > 0)
                    elementType = genericArguments[0];
                else
                    throw new Exception("Unknown type of array in Locater");
            }

            PropertyInfo propertyInfo = elementType.GetProperty(elementPropertyName);
            if (propertyInfo == null)
                throw new Exception(elementPropertyName + " is not a property of type " + elementType.Name);

            // Create a return array.
            Array values = Array.CreateInstance(propertyInfo.PropertyType, list.Count);
            for (int i = 0; i < list.Count; i++)
                values.SetValue(propertyInfo.GetValue(list[i]), i);

            return values;
        }

        /// <summary>
        /// Returns the string representation of our value
        /// </summary>
        /// <returns></returns>
        public string ValueAsString()
        {
            if (this.DataType.IsArray)
                return ValueWithArrayHandling.ToString();
            else
                return AsString(this.Value);
        }

        /// <summary>
        /// Returns the string representation of a scalar value. 
        /// Uses InvariantCulture when converting doubles
        /// to ensure a consistent representation of Nan and Inf
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string AsString(object value)
        {
            if (value == null)
                return string.Empty;
            Type type = value.GetType();
            if (type == typeof(double))
                return ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            else if (value is Enum)
                return GetEnumDescription(value as Enum);
            else
                return value.ToString();
        }

        /// <summary>
        /// Gets or sets the value of the specified property with arrays converted to comma separated strings.
        /// </summary>
        public override object ValueWithArrayHandling
        {
            get
            {
                object value = this.Value;
                if (value == null)
                {
                    return string.Empty;
                }

                if (this.DataType.IsArray)
                {
                    string stringValue = string.Empty;
                    Array arr = value as Array;
                    if (arr == null)
                    {
                        return stringValue;
                    }

                    for (int j = 0; j < arr.Length; j++)
                    {
                        if (j > 0)
                        {
                            stringValue += ",";
                        }

                        Array arr2d = arr.GetValue(j) as Array;
                        if (arr2d == null)
                            stringValue += AsString(arr.GetValue(j));
                        else
                        {
                            for (int k = 0; k < arr2d.Length; k++)
                            {
                                if (k > 0)
                                {
                                    stringValue += " \r\n ";
                                }
                                stringValue += AsString(arr2d.GetValue(k));
                            }
                        }
                    }

                    value = stringValue;
                }

                return value;
            }
        }

        /// <summary>
        /// Returns true if the variable is writable
        /// </summary>
        public override bool Writable { get { return property.CanRead && property.CanWrite; } }

        /// <summary>
        /// Gets the display format for this property e.g. 'N3'. Can return null if not present.
        /// </summary>
        public string Format
        {
            get
            {
                DisplayAttribute displayFormatAttribute = ReflectionUtilities.GetAttribute(this.property, typeof(DisplayAttribute), false) as DisplayAttribute;
                if (displayFormatAttribute != null && displayFormatAttribute.Format != null)
                {
                    return displayFormatAttribute.Format;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the crop name of the property or null if this property isn't a crop one.
        /// </summary>
        public string CropName
        {
            get
            {
                if (this.Object is ISoilCrop)
                {
                    ISoilCrop soilCrop = this.Object as ISoilCrop;
                    if (soilCrop.Name.EndsWith("Soil"))
                        return soilCrop.Name.Substring(0, soilCrop.Name.Length - 4);
                    return soilCrop.Name;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the sum of all values in this array property if the property has been 
        /// marked as [DisplayTotal]. Otherwise return double.Nan
        /// </summary>
        public double Total
        {
            get
            {
                try
                {
                    DisplayAttribute displayFormatAttribute = ReflectionUtilities.GetAttribute(this.property, typeof(DisplayAttribute), false) as DisplayAttribute;
                    bool hasDisplayTotal = displayFormatAttribute != null && displayFormatAttribute.ShowTotal;
                    if (hasDisplayTotal && this.Value != null && (Units == "mm" || Units == "kg/ha"))
                    {
                        double sum = 0.0;
                        foreach (double doubleValue in this.Value as IEnumerable<double>)
                        {
                            if (doubleValue != MathUtilities.MissingValue)
                            {
                                sum += doubleValue;
                            }
                        }

                        return sum;
                    }
                }
                catch (Exception)
                {
                    return Double.NaN;
                }

                return double.NaN;
            }
        }

        /// <summary>
        /// Gets the associated display type for the related property.
        /// </summary>
        public override DisplayAttribute Display
        {
            get
            {
                return ReflectionUtilities.GetAttribute(this.property, typeof(DisplayAttribute), false) as DisplayAttribute;
            }
        }

        /// <summary>
        /// Set the value of this object via a string.
        /// </summary>
        /// <param name="value">The string value to set this property to</param>
        private void SetFromString(string value)
        {
            if (this.DataType.IsArray)
            {
                string[] stringValues = value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (this.DataType == typeof(double[]))
                {
                    this.Value = MathUtilities.StringsToDoubles(stringValues);
                }
                else if (this.DataType == typeof(int[]))
                {
                    this.Value = MathUtilities.StringsToDoubles(stringValues);
                }
                else if (this.DataType == typeof(string[]))
                {
                    this.Value = stringValues;
                }
                else
                {
                    throw new ApsimXException(null, "Invalid property type: " + this.DataType.ToString());
                }
            }
            else
            {
                if (this.DataType == typeof(double))
                {
                    this.Value = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                }
                else if (this.DataType == typeof(int))
                {
                    this.Value = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
                else if (this.DataType == typeof(string))
                {
                    this.property.SetValue(this.Object, value, null);
                }
                else if (this.DataType == typeof(bool))
                {
                    this.property.SetValue(this.Object, Convert.ToBoolean(value), null);
                }
                else if (this.DataType == typeof(DateTime))
                {
                    this.Value = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                }
                else if (this.DataType.IsEnum)
                {
                    this.Value = Enum.Parse(this.DataType, value);
                }
                else
                {
                    this.Value = value;
                }
            }
        }

        /// <summary>
        /// Return an attribute
        /// </summary>
        /// <param name="attributeType">Type of attribute to find</param>
        /// <returns>The attribute or null if not found</returns>
        public override Attribute GetAttribute(Type attributeType)
        {
            return ReflectionUtilities.GetAttribute(this.property, attributeType, false);
        }

        /// <summary>
        /// Convert the specified enum to a list of strings.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string[] EnumToStrings(object obj)
        {
            List<string> items = new List<string>();
            foreach (object e in obj.GetType().GetEnumValues())
            {
                Enum value = e as Enum;
                if (value != null)
                    items.Add(GetEnumDescription(value));
                else
                    items.Add(e.ToString());
            }
            return items.ToArray();
        }

        /// <summary>
        /// Parse the specified object to an enum. 
        /// Similar to Enum.Parse(), but this will check against the enum's description attribute.
        /// </summary>
        /// <param name="obj">Object to parse. Should probably be a string.</param>
        /// <param name="t">Enum in which we will try to find a matching member.</param>
        /// <returns>Enum member.</returns>
        public static Enum ParseEnum(Type t, object obj)
        {
            FieldInfo[] fields = t.GetFields();
            foreach (FieldInfo field in fields)
            {
                DescriptionAttribute description = field.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (description != null && description.ToString() == obj.ToString())
                    return field.GetValue(null) as Enum;
            }
            return Enum.Parse(t, obj.ToString()) as Enum;
        }
    }
}