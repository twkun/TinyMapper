﻿using System.Collections.Generic;
using TinyMapper.Builders.Assemblies;
using TinyMapper.Builders.Assemblies.Types;
using TinyMapper.DataStructures;

namespace TinyMapper
{
    public sealed class ObjectMapper
    {
        private static readonly IDynamicAssembly _assembly = DynamicAssemblyBuilder.Get();
        private static readonly Dictionary<TypePair, Mapper> _mappers = new Dictionary<TypePair, Mapper>();

        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            TargetMapperBuilder targetMapperBuilder = _assembly.GetTypeBuilder();
            Mapper mapper = targetMapperBuilder.Build(typePair);
            _mappers[typePair] = mapper;

            _assembly.Save();
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            Mapper mapper = _mappers[TypePair.Create<TSource, TTarget>()];
            var result = (TTarget)mapper.MapMembers(source, target);

            return result;
        }

        public static TTarget Map<TTarget>(object source)
        {
            Mapper mapper = _mappers[TypePair.Create(source.GetType(), typeof(TTarget))];
            var result = (TTarget)mapper.MapMembers(source);
            return result;
        }
    }
}
