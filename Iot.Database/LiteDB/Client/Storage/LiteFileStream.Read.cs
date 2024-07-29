﻿using System;
using System.IO;
using System.Linq;
using static Iot.Database.Constants;

namespace Iot.Database
{
    public partial class LiteFileStream<TFileId> : Stream
    {
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_mode != FileAccess.Read) throw new NotSupportedException();

            var bytesLeft = count;

            while (_currentChunkData != null && bytesLeft > 0)
            {
                var bytesToCopy = Math.Min(bytesLeft, _currentChunkData.Length - _positionInChunk);

                Buffer.BlockCopy(_currentChunkData, _positionInChunk, buffer, offset, bytesToCopy);

                _positionInChunk += bytesToCopy;
                bytesLeft -= bytesToCopy;
                offset += bytesToCopy;
                _streamPosition += bytesToCopy;

                if (_positionInChunk >= _currentChunkData.Length)
                {
                    _positionInChunk = 0;

                    _currentChunkData = this.GetChunkData(++_currentChunkIndex);
                }
            }

            return count - bytesLeft;
        }

        private byte[] GetChunkData(int index)
        {
            // check if there is no more chunks in this file
            var chunk = _chunks
                .FindOne("_id = { f: @0, n: @1 }", _fileId, index);

            // if chunk is null there is no more chunks
            return chunk?["data"].AsBinary;
        }

        private void SetReadStreamPosition(long newPosition)
        {
            if (newPosition < 0 && newPosition > Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            _streamPosition = newPosition;

            // calculate new chunk position
            _currentChunkIndex = (int)_streamPosition / MAX_CHUNK_SIZE;
            _positionInChunk = (int)_streamPosition % MAX_CHUNK_SIZE;

            // get current chunk
            _currentChunkData = this.GetChunkData(_currentChunkIndex);
        }
    }
}