﻿using System.Collections.Generic;

namespace Dasync.Serialization
{
    public class StandardSerializerFactory : IStandardSerializerFactory
    {
        private readonly ITypeSerializerHelper _typeSerializerHelper;
        private readonly IObjectDecomposerSelector _decomposerSelector;
        private readonly IObjectComposerSelector _composerSelector;

        public StandardSerializerFactory(
            ITypeSerializerHelper typeSerializerHelper,
            IEnumerable<IObjectDecomposerSelector> decomposerSelectors,
            IEnumerable<IObjectComposerSelector> composerSelectors)
        {
            _typeSerializerHelper = typeSerializerHelper;
            _decomposerSelector = new ObjectDecomposerSelectorChain(decomposerSelectors);
            _composerSelector = new ObjectComposerSelectorChain(composerSelectors);
        }

        public ISerializer Create(
            IValueWriterFactory valueWriterFactory,
            IValueReaderFactory valueReaderFactory)
        {
            if (valueWriterFactory is IValueTextWriterFactory valueTextWriterFactory &&
                valueReaderFactory is IValueTextReaderFactory valueTextReaderFactory)
                return new StandardTextSerializer(
                    valueTextWriterFactory,
                    valueTextReaderFactory,
                    _decomposerSelector,
                    _composerSelector,
                    _typeSerializerHelper);

            return new StandardSerializer(
                valueWriterFactory,
                valueReaderFactory,
                _decomposerSelector,
                _composerSelector,
                _typeSerializerHelper);
        }
    }
}
