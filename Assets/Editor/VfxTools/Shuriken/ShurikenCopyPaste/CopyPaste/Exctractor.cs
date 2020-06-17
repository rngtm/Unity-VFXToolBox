using System.Linq;
using System.Reflection;

namespace VfxTools.ShurikenCopyPaste
{
    public static class Exctractor
    {
        /** ********************************************************************************
        * @summary オブジェクトのメンバの数値を取得
        ***********************************************************************************/
        public static MemberValue[] GetModuleValues(object particleSystemModule)
        {
            var objType = particleSystemModule.GetType();
            var memberInfos = particleSystemModule.GetType().GetMembers(
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.DeclaredOnly // 指定した型の階層のレベルで宣言されたメンバーのみ対象
                    | BindingFlags.FlattenHierarchy
                )
                .Where(m => m.MemberType == MemberTypes.Field | m.MemberType == MemberTypes.Property)
                .ToArray();

            var memberValues = new MemberValue[memberInfos.Count()];
            for (var index = 0; index < memberInfos.Length; index++)
            {
                var memberInfo = memberInfos[index];
                memberValues[index] = new MemberValue
                {
                    MemberType = memberInfo.MemberType,
                };

                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Property:
                    {
                        var prop = objType.GetProperty(memberInfo.Name);
                        memberValues[index].Value = prop?.GetValue(particleSystemModule);
                        memberValues[index].MemberName = memberInfo.Name;
                        break;
                    }
                    case MemberTypes.Field:
                    {
                        var field = objType.GetField(memberInfo.Name);
                        memberValues[index].Value = field?.GetValue(particleSystemModule);
                        memberValues[index].MemberName = memberInfo.Name;
                        break;
                    }
                    default:
                    {
                        memberValues[index].Value = memberInfo;
                        memberValues[index].MemberName = memberInfo.Name;
                        break;
                    }
                }
            }

            return memberValues;
        }

        /** ********************************************************************************
        * @summary オブジェクトのメンバの数値を設定
        ***********************************************************************************/
        public static void SetModuleValue(object particleSystemModule, ModuleCopyPasteBase moduleCopyPasteBase)
        {
            var flag = BindingFlags.Instance
                       | BindingFlags.Public
                       | BindingFlags.NonPublic
                       | BindingFlags.DeclaredOnly;
            MemberValue[] memberValues = moduleCopyPasteBase.GetMemberValues().ToArray();
            var particleModuleType = particleSystemModule.GetType();
            for (var index = 0; index < memberValues.Length; index++)
            {
                var value = memberValues.ElementAt(index).Value;
                switch (memberValues[index].MemberType)
                {
                    case MemberTypes.Property:
                    {
                        var prop = particleModuleType.GetProperty(memberValues[index].MemberName, flag);
                        if (prop != null && prop.CanWrite)
                        {
                            prop.SetValue(particleSystemModule, value);
                        }
                        break;
                    }
                    case MemberTypes.Field:
                    {
                        var field = particleModuleType.GetField(memberValues[index].MemberName, flag);
                        field?.SetValue(particleSystemModule, value);
                        break;
                    }
                }
            }
        }

    }
}