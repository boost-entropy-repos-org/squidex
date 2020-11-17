﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Squidex.Infrastructure.Json.Objects
{
    public class JsonObject : IReadOnlyDictionary<string, IJsonValue>, IJsonValue, IEquatable<JsonObject>
    {
        private readonly Dictionary<string, IJsonValue> inner;

        public IJsonValue this[string key]
        {
            get
            {
                return inner[key];
            }
            set
            {
                Guard.NotNull(key, nameof(key));

                inner[key] = value ?? JsonValue.Null;
            }
        }

        public IEnumerable<string> Keys
        {
            get { return inner.Keys; }
        }

        public IEnumerable<IJsonValue> Values
        {
            get { return inner.Values; }
        }

        public int Count
        {
            get { return inner.Count; }
        }

        public JsonValueType Type
        {
            get { return JsonValueType.Array; }
        }

        public JsonObject()
        {
            inner = new Dictionary<string, IJsonValue>();
        }

        public JsonObject(JsonObject obj)
        {
            inner = new Dictionary<string, IJsonValue>(obj.inner);
        }

        private JsonObject(Dictionary<string, IJsonValue> source)
        {
            inner = source;
        }

        public JsonObject Add(string key, object? value)
        {
            return Add(key, JsonValue.Create(value));
        }

        public JsonObject Add(string key, IJsonValue? value)
        {
            inner[key] = value ?? JsonValue.Null;

            return this;
        }

        public void Clear()
        {
            inner.Clear();
        }

        public bool Remove(string key)
        {
            return inner.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return inner.ContainsKey(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IJsonValue value)
        {
            return inner.TryGetValue(key, out value!);
        }

        public IEnumerator<KeyValuePair<string, IJsonValue>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as JsonObject);
        }

        public bool Equals(IJsonValue? other)
        {
            return Equals(other as JsonObject);
        }

        public bool Equals(JsonObject? other)
        {
            return other != null && inner.EqualsDictionary(other.inner);
        }

        public override int GetHashCode()
        {
            return inner.DictionaryHashCode();
        }

        public IJsonValue Clone()
        {
            return new JsonObject(this.ToDictionary(x => x.Key, x => x.Value.Clone()));
        }

        public string ToJsonString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return $"{{{string.Join(", ", this.Select(x => $"\"{x.Key}\":{x.Value.ToJsonString()}"))}}}";
        }

        public bool TryGet(string pathSegment, [MaybeNullWhen(false)] out IJsonValue result)
        {
            Guard.NotNull(pathSegment, nameof(pathSegment));

            return TryGetValue(pathSegment, out result!);
        }
    }
}
