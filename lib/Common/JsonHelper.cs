using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace GLT
{
    [_DebuggerStepThrough]
    public static class JsonHelper
    {
        #region Serialize / Deserialize

        public static JObject ToJObject(string json_string)
        {
            if (string.IsNullOrEmpty(json_string)) return null;
            using (StringReader r1 = new StringReader(json_string))
            using (JsonReader r2 = CreateReader(r1))
                return JObject.Load(r2);
        }

        public static JArray ToJArray(string json_string)
        {
            if (string.IsNullOrEmpty(json_string)) return null;
            using (StringReader r1 = new StringReader(json_string))
            using (JsonReader r2 = CreateReader(r1))
                return JArray.Load(r2);
        }



        public static string SerializeObject(object value, Formatting formatting = Formatting.None, bool quoteName = true, char? quoteChar = null)
        {
            if (value == null) return null;
            var str = new StringBuilder();
            using (StringWriter w1 = new StringWriter(str))
            using (var w2 = CreateWriter(w1, formatting, quoteName, quoteChar))
            {
                var serializer = RentJsonSerializer();
                try { serializer.Serialize(w2, value); }
                finally { ReturnJsonSerializer(serializer); }
            }
            return str.ToString();
        }

        public static T DeserializeObject<T>(string json_string)
        {
            if (json_string == null) return default(T);
            using (StringReader r1 = new StringReader(json_string))
            using (JsonReader r2 = CreateReader(r1))
            {
                var serializer = RentJsonSerializer();
                try { return serializer.Deserialize<T>(r2); }
                finally { ReturnJsonSerializer(serializer); }
            }
        }


        //public static void SerializeObject(TextWriter writer, object value, Formatting formatting = Formatting.None, bool quoteName = true, char? quoteChar = null)
        //{
        //    if (value == null) return;
        //    using (var w = CreateWriter(writer, formatting, quoteName, quoteChar))
        //    using (var serializer = _Serializer.GetInstance())
        //        serializer.Serialize(w, value);
        //}




        //public static bool DeserializeObject(Type type, string json_string, out object result)
        //{
        //    if (string.IsNullOrEmpty(json_string))
        //    {
        //        result = null;
        //        return false;
        //    }
        //    using (StringReader r1 = new StringReader(json_string))
        //    using (JsonReader r2 = CreateReader(r1))
        //    using (var serializer = _Serializer.GetInstance())
        //        result = serializer.Deserialize(r2, type);
        //    return result != null;
        //}

        //public static object DeserializeObject(Type type, string json_string)
        //{
        //    object result;
        //    DeserializeObject(type, json_string, out result);
        //    return result;
        //}

        //public static bool DeserializeObject(Type type, TextReader textReader, out object result)
        //{
        //    result = DeserializeObject(type, textReader);
        //    return result != null;
        //}

        //public static object DeserializeObject(Type type, TextReader textReader)
        //{
        //    using (JsonReader r2 = CreateReader(textReader))
        //    using (var serializer = _Serializer.GetInstance())
        //        return serializer.Deserialize(r2, type);
        //}




        public static void SerializeObject(JsonWriter writer, object value)
        {
            if (writer == null) return;
            if (value == null) return;
            var serializer = RentJsonSerializer();
            try { serializer.Serialize(writer, value); }
            finally { ReturnJsonSerializer(serializer); }
        }

        public static T DeserializeObject<T>(TextReader textReader)
        {
            if (textReader == null) return default(T);
            using (JsonReader r2 = CreateReader(textReader))
            {
                var serializer = RentJsonSerializer();
                try { return serializer.Deserialize<T>(r2); }
                finally { ReturnJsonSerializer(serializer); }
            }
        }



        public static bool PopulateObject(string json_string, object obj)
        {
            if (json_string == null) return false;
            using (StringReader r1 = new StringReader(json_string))
            using (JsonReader r2 = CreateReader(r1))
            {
                var serializer = RentJsonSerializer();
                try { serializer.Populate(r2, obj); }
                finally { ReturnJsonSerializer(serializer); }
            }
            return true;
        }

        #endregion

        #region Serializer / Reader / Writer / ContractResolver

        private class _ArrayPool : IArrayPool<char>
        {
            static ArrayPool<char> _pool = ArrayPool<char>.Create();
            public static _ArrayPool Instance = new _ArrayPool();
            //static Dictionary<int, Queue<char[]>> _pool = new Dictionary<int, Queue<char[]>>();

            char[] IArrayPool<char>.Rent(int minimumLength)
            {
                lock (_pool) return _pool.Rent(minimumLength);
                //Queue<char[]> queue;
                //lock (_pool)
                //{
                //    if (_pool.TryGetValue(minimumLength, out queue))
                //        if (queue.Count > 0)
                //            return queue.Dequeue();
                //}
                //return new char[minimumLength];
            }

            void IArrayPool<char>.Return(char[] array)
            {
                lock (_pool) _pool.Return(array);
                //Queue<char[]> queue;
                //lock (_pool)
                //{
                //    if (!_pool.TryGetValue(array.Length, out queue))
                //        queue = _pool[array.Length] = new Queue<char[]>();
                //    queue.Enqueue(array);
                //}
            }
        }

        public static JsonTextReader CreateReader(TextReader reader)
        {
            return new JsonTextReader(reader) { ArrayPool = _ArrayPool.Instance };
            //return new _Reader(reader);
        }
        public static JsonTextWriter CreateWriter(TextWriter writer, Formatting formatting = Formatting.None, bool quoteName = true, char? quoteChar = null)
        {
            //var w = new _Writer(writer);
            var w = new JsonTextWriter(writer) { ArrayPool = _ArrayPool.Instance };
            if (quoteChar.HasValue)
                w.QuoteChar = quoteChar.Value;
            w.QuoteName = quoteName;
            w.Formatting = formatting;
            return w;
        }

        //[_DebuggerStepThrough]
        //private class _Reader : JsonTextReader
        //{
        //    public _Reader(TextReader reader) : base(reader)
        //    {
        //        base.ArrayPool = _ArrayPool.Instance;
        //    }

        //    public override bool Read()
        //    {
        //        bool result = base.Read();
        //        if (this.TokenType == JsonToken.Date && this.Value is DateTime)
        //        {
        //            try
        //            {
        //                DateTime t = (DateTime)this.Value;
        //                base.SetToken(this.TokenType, t.ToLocalTime());
        //            }
        //            catch { }
        //        }
        //        return result;
        //    }

        //    //T _Read<T>(Func<T> r) where T : class
        //    //{
        //    //    try { return r(); }
        //    //    catch { SetToken(JsonToken.Null); return null; }
        //    //}

        //    //T? _Read<T>(Func<T?> r) where T : struct
        //    //{
        //    //    try { return r(); }
        //    //    catch { SetToken(JsonToken.Null); return null; }
        //    //}

        //    //public override string ReadAsString()
        //    //{
        //    //    return base.ReadAsString();
        //    //}

        //    //public override bool? ReadAsBoolean()
        //    //{
        //    //    return _Read(base.ReadAsBoolean);
        //    //}

        //    //public override DateTime? ReadAsDateTime()
        //    //{
        //    //    return _Read(base.ReadAsDateTime);
        //    //}

        //    //public override DateTimeOffset? ReadAsDateTimeOffset()
        //    //{
        //    //    return _Read(base.ReadAsDateTimeOffset);
        //    //}

        //    //public override decimal? ReadAsDecimal()
        //    //{
        //    //    return _Read(base.ReadAsDecimal);
        //    //}

        //    //public override double? ReadAsDouble()
        //    //{
        //    //    return _Read(base.ReadAsDouble);
        //    //}

        //    //public override int? ReadAsInt32()
        //    //{
        //    //    return _Read(base.ReadAsInt32);
        //    //}

        //    //public override byte[] ReadAsBytes()
        //    //{
        //    //    return _Read(base.ReadAsBytes);
        //    //}
        //}

        //[_DebuggerStepThrough]
        //private class _Writer : JsonTextWriter
        //{
        //    public _Writer(TextWriter textWriter)
        //        : base(textWriter)
        //    {
        //        base.ArrayPool = _ArrayPool.Instance;
        //        base.QuoteChar = '\"';
        //        base.QuoteName = true;
        //        base.Formatting = Formatting.None;
        //    }

        //    public override void WriteValue(DateTime value)
        //    {   // protocol 的日期一律轉換成 utc
        //        base.WriteValue(value.ToUniversalTime());
        //    }

        //    //public override void WriteValue(DateTime? value)
        //    //{
        //    //    DateTime? value2;
        //    //    if (value.HasValue)
        //    //        value2 = value.Value.ToUniversalTime();
        //    //    else
        //    //        value2 = value;
        //    //    base.WriteValue(value2);
        //    //}
        //}

        public static void ConfigureMvcJsonOptions(this JsonSerializerSettings settings)
        {
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = _ContractResolver.Instance;
        }

        private static Queue<JsonSerializer> _serializers = new Queue<JsonSerializer>();
        private static JsonSerializer RentJsonSerializer()
        {
            lock (_serializers)
                if (_serializers.Count > 0)
                    return _serializers.Dequeue();
            var settings = new JsonSerializerSettings();
            ConfigureMvcJsonOptions(settings);
            var result = JsonSerializer.Create(settings);
            return result;
        }

        private static void ReturnJsonSerializer(JsonSerializer obj)
        {
            lock (_serializers)
                _serializers.Enqueue(obj);
        }

        //[_DebuggerStepThrough]
        //private class _Serializer : JsonSerializer, IDisposable
        //{
        //    private static Queue<_Serializer> _pooling = new Queue<_Serializer>();

        //    public static _Serializer GetInstance()
        //    {
        //        lock (_pooling)
        //            if (_pooling.Count > 0)
        //                return _pooling.Dequeue();
        //        return new _Serializer();
        //    }

        //    void IDisposable.Dispose()
        //    {
        //        lock (_pooling)
        //            _pooling.Enqueue(this);
        //    }

        //    private _Serializer()
        //    {
        //        base.NullValueHandling = NullValueHandling.Ignore;
        //        base.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        //        base.ContractResolver = _ContractResolver.Instance;
        //    }
        //}


        //[_DebuggerStepThrough]
        //private class _Serializer : JsonSerializer, IDisposable
        //{
        //    private static Queue<_Serializer> _pooling1 = new Queue<_Serializer>();
        //    private static Queue<_Serializer> _pooling2 = new Queue<_Serializer>();

        //    public static _Serializer GetInstance(bool expandoObject = false)
        //    {
        //        Queue<_Serializer> _pooling = expandoObject ? _pooling2 : _pooling1;
        //        lock (_pooling)
        //            if (_pooling.Count > 0)
        //                return _pooling.Dequeue();
        //        return new _Serializer(expandoObject);
        //    }

        //    void IDisposable.Dispose()
        //    {
        //        Queue<_Serializer> _pooling = _expandoObject ? _pooling2 : _pooling1;
        //        lock (_pooling)
        //            _pooling.Enqueue(this);
        //    }

        //    private readonly bool _expandoObject;

        //    private _Serializer(bool expandoObject = false)
        //    {
        //        this._expandoObject = expandoObject;
        //        if (expandoObject)
        //            base.Converters.Add(new ExpandoObjectConverter());
        //        base.NullValueHandling = NullValueHandling.Ignore;
        //        base.ContractResolver = _ContractResolver.Instance;
        //    }
        //}

        [_DebuggerStepThrough]
        private class _ContractResolver : DefaultContractResolver
        {
            public static readonly _ContractResolver Instance = new _ContractResolver();

            //public bool MapName = true;

            //protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
            //{
            //    JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);
            //    UnderlyingValueInDictionaryKeyAttribute.Apply(contract);
            //    return contract;
            //}

            //protected override JsonContract CreateContract(Type objectType)
            //{
            //    JsonContract c = base.CreateContract(objectType);
            //    if (objectType.IsEnum)
            //    {
            //        c.Converter = c.Converter ?? StringEnumAttribute.GetStringEnumConverter(objectType.GetCustomAttribute<StringEnumAttribute>()); ;
            //        //if (c.Converter is json.StringEnumConverter) { }
            //        //else
            //        //    c.Converter = StringEnumAttribute.GetStringEnumConverter(objectType.GetCustomAttribute<StringEnumAttribute>());
            //    }
            //    return c;
            //}

            //protected override JsonContract CreateContract(Type objectType)
            //{
            //    var c = base.CreateContract(objectType);
            //    return c;
            //}

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty p = base.CreateProperty(member, memberSerialization);
                //if (MapName == false)
                //    p.PropertyName = member.Name;
                Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                if (t.IsEnum)
                {
                    JsonContract cc = this.ResolveContract(t);
                    p.Converter = p.Converter ?? cc.Converter ?? StringEnumAttribute.GetStringEnumConverter(member.GetCustomAttribute<StringEnumAttribute>() ?? t.GetTypeInfo().GetCustomAttribute<StringEnumAttribute>());
                    //p.Converter = p.Converter ?? StringEnumAttribute.GetStringEnumConverter(member.GetCustomAttribute<StringEnumAttribute>() ?? t.GetCustomAttribute<StringEnumAttribute>());
                }
                //else if (t == typeof(string))
                //{
                //    p.Converter = p.Converter ?? new StringJsonConverter(member, p);
                //}
                //else if (t == typeof(DateTime))
                //{
                //    p.Converter = p.Converter ?? new UTCDateTimeConverter();
                //}
                //else if (t == typeof(UserName))
                //{
                //    ;
                //    p.ShouldSerialize = v =>
                //    {
                //        return true;
                //    };
                //}
                //if (p.Converter == null)
                //{
                //    if (t == typeof(Guid))
                //        p.Converter = new GuidJsonConverter();
                //    //    else if (t == typeof(Boolean))
                //    //        p.MemberConverter = new BooleanJsonConverter();
                //    //    else if (t == typeof(string))
                //    //        p.MemberConverter = new StringJsonConverter(member, p);
                //}
                return p;
            }
        }

        public static JsonContract ResolveContract<T>() => _ContractResolver.Instance.ResolveContract(typeof(T));

        //private class _Creator<T> : IDisposable
        //{
        //    public static readonly _Creator<T> Instance = new _Creator<T>();

        //    private JsonContract jsonContract;
        //    //private Type objectType;
        //    private Func<object> OriginalCreator;

        //    private _Creator() { }

        //    Dictionary<Thread, Func<T>> cb = new Dictionary<Thread, Func<T>>();

        //    object Creator()
        //    {
        //        if (cb.TryGetValue(Thread.CurrentThread, out Func<T> creator, syncLock: true))
        //            return creator();
        //        return OriginalCreator();
        //    }

        //    void IDisposable.Dispose()
        //    {
        //        cb.TryRemove(Thread.CurrentThread, syncLock: true);
        //    }

        //    public IDisposable HandleObjectCreation(Func<T> creator)
        //    {
        //        if (typeof(T).IsValueType) return this;
        //        lock (cb)
        //        {
        //            if (jsonContract == null)
        //            {
        //                jsonContract = _ContractResolver.Instance.ResolveContract(typeof(T));
        //                OriginalCreator = jsonContract.DefaultCreator;
        //                jsonContract.DefaultCreator = Creator;
        //            }
        //            cb[Thread.CurrentThread] = creator;
        //        }
        //        return this;
        //    }
        //}

        //public static IDisposable HandleObjectCreation<T>(Func<T> creator) => _Creator<T>.Instance.HandleObjectCreation(creator);

        #endregion

        #region Converters

        //public class UTCDateTimeConverter : Newtonsoft.Json.JsonConverter
        //{
        //    private TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        //    public override bool CanConvert(Type objectType)
        //    {
        //        return objectType == typeof(DateTime);
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        if (reader.Value == null) return null;
        //        var pacificTime = DateTime.Parse(reader.Value.ToString());
        //        return TimeZoneInfo.ConvertTimeToUtc(pacificTime, pacificZone);
        //    }

        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        writer.WriteValue(TimeZoneInfo.ConvertTimeFromUtc((DateTime)value, pacificZone));
        //    }
        //}


        //[_DebuggerStepThrough]
        //public class BooleanJsonConverter : JsonConverter
        //{
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        if (reader.Value is Boolean)
        //            return reader.Value;
        //        string input = reader.Value as string;
        //        if (string.IsNullOrEmpty(input))
        //            return null;
        //        input = input.ToLower();
        //        if (Regex.IsMatch(input, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
        //            return false;
        //        if (Regex.IsMatch(input, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
        //            return true;
        //        return null;
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public class StringAttribute : Attribute
        {
            public bool Trim = false;
            public bool Empty = false;
        }

        [_DebuggerStepThrough]
        public class StringJsonConverter : JsonConverter
        {
            static int _id = 0;
            readonly int ID = Interlocked.Increment(ref _id);
            readonly JsonProperty p;
            readonly StringAttribute a;

            public StringJsonConverter() : this(null, null) { }
            public StringJsonConverter(MemberInfo member, JsonProperty p)
            {
                this.p = p;
                if (member != null)
                    foreach (StringAttribute a in member.GetCustomAttributes<StringAttribute>(true))
                        this.a = a;
                this.a = this.a ?? new StringAttribute() { Trim = true, Empty = false };
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                object value = serializer.Deserialize(reader);
                string s = value as string;
                if (s != null)
                {
                    if (this.a.Trim)
                        s = s.Trim();
                    if ((s == string.Empty) && (this.a.Empty == false))
                        s = null;
                }
                return s;
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }

        //[_DebuggerStepThrough]
        //public class Int32JsonConverter : JsonConverter
        //{
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //[_DebuggerStepThrough]
        //public class GuidJsonConverter : JsonConverter
        //{
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        serializer.Serialize(writer, value);
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        try { return new Guid(reader.Value as string); }
        //        catch { if (objectType.IsNullable()) return null; else return Guid.Empty; }
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Enum)]
        public class UnderlyingValueInDictionaryKeyAttribute : Attribute
        {
            static List<UnderlyingValueInDictionaryKeyAttribute> cache = new List<UnderlyingValueInDictionaryKeyAttribute>();

            public static void Apply(JsonDictionaryContract contract)
            {
                if (contract == null) return;
                if (!contract.DictionaryKeyType.GetTypeInfo().IsEnum) return;
                //if (contract.PropertyNameResolver != null) return;
                lock (cache)
                {
                    foreach (UnderlyingValueInDictionaryKeyAttribute a in cache)
                    {
                        if (a.type != contract.DictionaryKeyType) continue;
                        contract.DictionaryKeyResolver = a.ResolvePropertyName;
                        //contract.PropertyNameResolver = a.ResolvePropertyName;
                        return;
                    }
                    foreach (UnderlyingValueInDictionaryKeyAttribute a in contract.DictionaryKeyType.GetTypeInfo().GetCustomAttributes<UnderlyingValueInDictionaryKeyAttribute>(true))
                    {
                        a.type = contract.DictionaryKeyType;
                        contract.DictionaryKeyResolver = a.ResolvePropertyName;
                        //contract.PropertyNameResolver = a.ResolvePropertyName;
                        cache.Add(a);
                        return;
                    }
                }
            }

            Type type;
            string ResolvePropertyName(string propertyName)
            {
                if (Enum.IsDefined(type, propertyName))
                {
                    object o1 = Enum.Parse(type, propertyName);
                    object o2 = Convert.ChangeType(o1, Enum.GetUnderlyingType(type));
                    return Convert.ToString(o2);
                }
                return propertyName;
            }
        }

        [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
        public class StringEnumAttribute : Attribute
        {
            public bool AsString { get; set; }

            public StringEnumAttribute(bool asString = true)
            {
                this.AsString = asString;
            }

            internal static StringEnumConverter GetStringEnumConverter(StringEnumAttribute attr)
            {
                if (attr == null)
                    return new GLT.JsonHelper.StringEnumConverter();
                else if (attr.AsString)
                    return new GLT.JsonHelper.StringEnumConverter();
                return null;
            }
        }

        public class StringEnumConverter : Newtonsoft.Json.Converters.StringEnumConverter
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                try { return base.ReadJson(reader, objectType, existingValue, serializer); }
                catch { return null; }
            }
        }
        #endregion
    }
}