﻿//
// System.Security.Cryptography.HashAlgorithm.cs
//
// Authors:
//	Matthew S. Ford (Matthew.S.Ford@Rose-Hulman.Edu)
//	Sebastien Pouliot (sebastien@ximian.com)
//
// Copyright 2001 by Matthew S. Ford.
// Portions (C) 2002 Motus Technologies Inc. (http://www.motus.com)
// Copyright (C) 2004-2006 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;

namespace Tweetinvi.Security
{
    public abstract class HashAlgorithm : ICryptoTransform
    {
        protected internal byte[] _hashValue;
        protected int _hashSizeValue;
        protected int _state;
        private bool _disposed;

        protected HashAlgorithm()
        {
            _disposed = false;
        }

        public virtual bool CanTransformMultipleBlocks => true;

        public virtual bool CanReuseTransform => true;

        public void Clear()
        {
            // same as System.IDisposable.Dispose() which is documented
            Dispose(true);
        }

        public byte[] ComputeHash(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            return ComputeHash(buffer, 0, buffer.Length);
        }

        public byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                throw new ObjectDisposedException("HashAlgorithm");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "< 0");
            if (count < 0)
                throw new ArgumentException("count", "< 0");
            // ordered to avoid possible integer overflow
            if (offset > buffer.Length - count)
            {
                throw new ArgumentException("offset + count", "Overflow");
            }

            HashCore(buffer, offset, count);
            _hashValue = HashFinal();
            Initialize();

            return _hashValue;
        }

        public byte[] ComputeHash(Stream inputStream)
        {
            // don't read stream unless object is ready to use
            if (_disposed)
                throw new ObjectDisposedException("HashAlgorithm");

            byte[] buffer = new byte[4096];
            int len = inputStream.Read(buffer, 0, 4096);
            while (len > 0)
            {
                HashCore(buffer, 0, len);
                len = inputStream.Read(buffer, 0, 4096);
            }
            _hashValue = HashFinal();
            Initialize();
            return _hashValue;
        }

        public static HashAlgorithm Create()
        {
#if FULL_AOT_RUNTIME
			return new System.Security.Cryptography.SHA1CryptoServiceProvider ();
#else
            return Create("System.Security.Cryptography.HashAlgorithm");
#endif
        }

        public static HashAlgorithm Create(string hashName)
        {
            return Activator.CreateInstance<SHA1CryptoServiceProvider>();
        }

        public virtual byte[] Hash
        {
            get
            {
                if (_hashValue == null)
                {
                    throw new Exception();
                }
                return _hashValue;
            }
        }

        protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

        protected abstract byte[] HashFinal();

        public virtual int HashSize => _hashSizeValue;

        public abstract void Initialize();

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }

        public virtual int InputBlockSize => 1;

        public virtual int OutputBlockSize => 1;

#if NET_4_0
		public void Dispose ()
#else
        void IDisposable.Dispose()
#endif
        {
            Dispose(true);
            GC.SuppressFinalize(this);  // Finalization is now unnecessary
        }

        // LAMESPEC: outputBuffer is optional in 2.0 (i.e. can be null).
        // However a null outputBuffer would throw a ExecutionEngineException under 1.x
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer");

            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException("inputOffset", "< 0");
            if (inputCount < 0)
                throw new ArgumentException("inputCount");

            // ordered to avoid possible integer overflow
            if ((inputOffset < 0) || (inputOffset > inputBuffer.Length - inputCount))
                throw new ArgumentException("inputBuffer");

            if (outputBuffer != null)
            {
                if (outputOffset < 0)
                {
                    throw new ArgumentOutOfRangeException("outputOffset", "< 0");
                }
                // ordered to avoid possible integer overflow
                if (outputOffset > outputBuffer.Length - inputCount)
                {
                    throw new Exception("outputOffset + inputCount");
                }
            }

            HashCore(inputBuffer, inputOffset, inputCount);

            if (outputBuffer != null)
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);

            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer");
            if (inputCount < 0)
                throw new ArgumentException("inputCount");
            // ordered to avoid possible integer overflow
            if (inputOffset > inputBuffer.Length - inputCount)
            {
                throw new ArgumentException("inputOffset + inputCount");
            }

            byte[] outputBuffer = new byte[inputCount];

            // note: other exceptions are handled by Buffer.BlockCopy
            Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, 0, inputCount);

            HashCore(inputBuffer, inputOffset, inputCount);
            _hashValue = HashFinal();
            Initialize();

            return outputBuffer;
        }
    }
}
