﻿using System;
using System.IO;

namespace ProtoBuf.Meta
{
    public abstract class TypeModel
    {
        public void Serialize(Stream dest, object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            int key = GetKey(value.GetType());
            using (ProtoWriter writer = new ProtoWriter(dest, this))
            {
                Serialize(key, value, writer);
            }
        }
        public object Deserialize(Stream source, object value, Type type)
        {
            if (type == null)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("type");
                } else {
                    type = value.GetType();
                }
            }
#if !FX11
            type = Nullable.GetUnderlyingType(type) ?? type;
#endif
            int key = GetKey(type);
            using (ProtoReader reader = new ProtoReader(source, this))
            {
                return Deserialize(key, value, reader);
            }
        }
        public static RuntimeTypeModel Create(string name)
        {
            return new RuntimeTypeModel(name, false);
        }
        protected abstract int GetKey(Type type);
        protected internal abstract void Serialize(int key, object value, ProtoWriter dest);
        protected internal abstract object Deserialize(int key, object value, ProtoReader source);

        //internal ProtoSerializer Create(IProtoSerializer head)
        //{
        //    return new RuntimeSerializer(head, this);
        //}
        //internal ProtoSerializer Compile
    }

}
