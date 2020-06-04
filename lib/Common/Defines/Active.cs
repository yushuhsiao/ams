using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace GLT
{
    [JsonHelper.StringEnum(AsString = false)]
    public enum ActiveState : sbyte
    {
        Disabled = 0,
        Active = 1,
        Hidden = -1,
    }

    //[DebuggerStepThrough]
    //public struct Active
    //{
    //    public sbyte Value
    //    {
    //        get;
    //        set;
    //    }

    //    public Active(bool value)
    //    {
    //        if (value) this.Value = 1; else this.Value = 0;
    //    }
    //    public static implicit operator Active(bool n)
    //    {
    //        return new Active(n);
    //    }
    //    public static implicit operator bool(Active n)
    //    {
    //        return n.Value == 1;
    //    }

    //    //public Active(sbyte value)
    //    //{
    //    //    this.Value = value;
    //    //}
    //    //public static implicit operator Active(sbyte n)
    //    //{
    //    //    return new Active(n);
    //    //}
    //    //public static implicit operator sbyte(Active n)
    //    //{
    //    //    return n.Value;
    //    //}
    //}

    //public enum AccountState
    //{
    //    Disabled = 0,       // 不能登入
    //    Active = 1,         // 正常
    //    Locked = 2,         // 帳務鎖定
    //}

    //class ActiveStateJsonConverter : json.StringEnumConverter
    //{
    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        string value = serializer.Deserialize(reader, typeof(string)) as string;
    //        bool n1;
    //        if (value.ToBoolean(out n1))
    //            return n1 ? ActiveState.Active : ActiveState.Disabled;
    //        ActiveState n2;
    //        if (value.ToEnum(out n2))
    //            return n2;
    //        return null;
    //        //return base.ReadJson(reader, objectType, existingValue, serializer);
    //    }
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        ActiveState n = (ActiveState)value;
    //        serializer.Serialize(writer, n == ActiveState.Active);
    //    }
    //}
    //[JsonConverter(typeof(ActiveStateJsonConverter))]

    //[Flags]
    //public enum Active3 : byte
    //{
    //    Disabled = Active1.Disabled,
    //    Accounts = 1 << 0,      // 可存提款
    //    Game = 1 << 1,          // 可玩遊戲及遊戲存提款    
    //}

}