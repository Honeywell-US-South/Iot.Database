﻿using System;
using System.Collections.Generic;
using System.Linq;
using Iot.Database.Engine;
using static Iot.Database.Constants;

namespace Iot.Database
{
    internal partial class SqlParser
    {
        /// <summary>
        /// DELETE {collection} WHERE {whereExpr}
        /// </summary>
        private BsonDataReader ParseDelete()
        {
            _tokenizer.ReadToken().Expect("DELETE");

            var collection = _tokenizer.ReadToken().Expect(TokenType.Word).Value;

            BsonExpression where = null;

            if (_tokenizer.LookAhead().Is("WHERE"))
            {
                // read WHERE
                _tokenizer.ReadToken();

                where = BsonExpression.Create(_tokenizer, BsonExpressionParserMode.Full, _parameters);
            }

            _tokenizer.ReadToken().Expect(TokenType.EOF, TokenType.SemiColon);

            _tokenizer.ReadToken();

            var result = _engine.DeleteMany(collection, where);

            return new BsonDataReader(result);
        }
    }
}