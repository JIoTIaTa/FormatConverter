using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumAtributes
{

    public abstract class BaseAttribute : Attribute
    {
        private readonly object _value;
        public BaseAttribute(object value) { this._value = value; }

        public object GetValue() { return this._value; }
        public class FileExtension : BaseAttribute { public FileExtension(string value) : base(value) { } }

        public class Description : BaseAttribute { public Description(string value) : base(value) { } }
    }

    public static class EnumExtensionMethods
    {
        public static string GetFileExtension(this Enum enumItem)
        {
            return enumItem.GetAttributeValue(typeof(BaseAttribute.FileExtension), string.Empty);
        }
        public static string GetDescription(this Enum enumItem)
        {
            return enumItem.GetAttributeValue(typeof(BaseAttribute.Description), string.Empty);
        }
    }

    public static class EnumAttributesBaseLogic
    {
        /// <param name="enumItem">Элемент перечисления</param>
        /// <param name="attributeType">Тип атрибута, значение которого хотим получить</param>
        /// <param name="defaultValue">
        /// Значение по-умолчанию, которое будет возвращено, если переданный
        /// элемент перечисления не помечен переданным атрибутом
        /// </param>
        /// <returns>Возвращает значение переданного атрибута у переданного элемента перечисления</returns>
        public static VAL GetAttributeValue<ENUM, VAL>(this ENUM enumItem, Type attributeType, VAL defaultValue)
        {
            var attribute = enumItem.GetType().GetField(enumItem.ToString()).GetCustomAttributes(attributeType, true)
                .Where(a => a is BaseAttribute)
                .Select(a => (BaseAttribute)a)
                .FirstOrDefault();

            return attribute == null ? defaultValue : (VAL)attribute.GetValue();
        }
    }
}
