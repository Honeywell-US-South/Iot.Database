﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iot.Database.Engine;
using static Iot.Database.Constants;

namespace Iot.Database
{
    internal partial class SqlParser
    {
        /// <summary>
        /// PRAGMA [DB_PARAM] = VALUE
        /// PRAGMA [DB_PARAM]
        /// </summary>
        private IBsonDataReader ParsePragma()
        {
            _tokenizer.ReadToken().Expect("PRAGMA");

            var name = _tokenizer.ReadToken().Expect(TokenType.Word).Value;

            var eof = _tokenizer.LookAhead();

            if (eof.Type == TokenType.EOF || eof.Type == TokenType.SemiColon)
            {
                _tokenizer.ReadToken();

                var result = _engine.Pragma(name);

                return new BsonDataReader(result);
            }
            else if (eof.Type == TokenType.Equals)
            {
                // read =
                _tokenizer.ReadToken().Expect(TokenType.Equals);

                // read <value>
                var reader = new JsonReader(_tokenizer);
                var value = reader.Deserialize();

                // read last ; \ <eof>
                _tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);

                var result = _engine.Pragma(name, value);

                return new BsonDataReader(result);
            }

            throw LiteException.UnexpectedToken(eof);
        }
    }
}