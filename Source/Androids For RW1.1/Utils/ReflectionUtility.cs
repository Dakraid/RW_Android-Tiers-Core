using System;

namespace Androids
{
    public static class ReflectionUtility
    {
        public static object CloneObjectShallowly(this object sourceObject)
        {
            var flag = sourceObject == null;
            object result;
            if (flag)
            {
                result = null;
            }
            else
            {
                var type = sourceObject.GetType();
                var isAbstract = type.IsAbstract;
                if (isAbstract)
                {
                    result = null;
                }
                else
                {
                    var flag2 = type.IsPrimitive || type.IsValueType || type.IsArray || type == typeof(string);
                    if (flag2)
                    {
                        result = sourceObject;
                    }
                    else
                    {
                        var obj = Activator.CreateInstance(type);
                        var flag3 = obj == null;
                        if (flag3)
                        {
                            result = null;
                        }
                        else
                        {
                            foreach (var fieldInfo in type.GetFields())
                            {
                                var flag4 = !fieldInfo.IsLiteral;
                                if (!flag4) continue;

                                var value = fieldInfo.GetValue(sourceObject);
                                fieldInfo.SetValue(obj, value);
                            }

                            result = obj;
                        }
                    }
                }
            }

            return result;
        }
    }
}