﻿using System.IO;
using Dasync.ValueContainer;

namespace Dasync.Serialization
{
    public class StandardSerializer : ISerializer
    {
        private readonly IValueWriterFactory _valueWriterFactory;
        private readonly IValueReaderFactory _valueReaderFactory;
        private readonly IObjectDecomposerSelector _decomposerSelector;
        private readonly IObjectComposerSelector _composerSelector;
        private readonly ITypeSerializerHelper _typeSerializerHelper;

        public StandardSerializer(
            IValueWriterFactory valueWriterFactory,
            IValueReaderFactory valueReaderFactory,
            IObjectDecomposerSelector decomposerSelector,
            IObjectComposerSelector composerSelector,
            ITypeSerializerHelper typeSerializerHelper)
        {
            _valueWriterFactory = valueWriterFactory;
            _valueReaderFactory = valueReaderFactory;
            _decomposerSelector = decomposerSelector;
            _composerSelector = composerSelector;
            _typeSerializerHelper = typeSerializerHelper;
        }

        public void Serialize(Stream stream, object @object)
        {
            if (@object == null)
                return;

            using (var valueWriter = _valueWriterFactory.Create(stream))
            {
                var serializer = new ValueContainerSerializer(_decomposerSelector, _typeSerializerHelper);
                serializer.Serialize(@object, valueWriter);
            }
        }

        public void Populate(Stream stream, IValueContainer target)
        {
            using (var valueReader = _valueReaderFactory.Create(stream))
            {
                var reconstructor = new ObjectReconstructor(_composerSelector, target, _typeSerializerHelper);
                valueReader.Read(reconstructor);
            }
        }
    }
}
