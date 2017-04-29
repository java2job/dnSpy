﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using dndbg.Engine;
using dnSpy.Contracts.Debugger.Code;
using dnSpy.Contracts.Debugger.DotNet.CorDebug.Code;
using dnSpy.Contracts.Metadata;
using dnSpy.Debugger.CorDebug.Impl;

namespace dnSpy.Debugger.CorDebug.Code {
	sealed class DbgDotNetNativeCodeLocationImpl : DbgDotNetNativeCodeLocation {
		public override string Type => PredefinedDbgCodeLocationTypes.DotNetCorDebugNative;
		public override ModuleId Module { get; }
		public override uint Token { get; }
		public override uint ILOffset { get; }
		public override DbgILOffsetMapping ILOffsetMapping { get; }
		public override ulong NativeMethodAddress { get; }
		public override uint NativeMethodOffset { get; }
		public DnDebuggerObjectHolder<CorCode> CorCode { get; }

		internal DbgBreakpointLocationFormatterImpl Formatter { get; set; }

		readonly DbgDotNetNativeCodeLocationFactory owner;

		public DbgDotNetNativeCodeLocationImpl(DbgDotNetNativeCodeLocationFactory owner, ModuleId module, uint token, uint ilOffset, DbgILOffsetMapping ilOffsetMapping, ulong nativeMethodAddress, uint nativeMethodOffset, DnDebuggerObjectHolder<CorCode> corCode) {
			this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
			Module = module;
			Token = token;
			ILOffset = ilOffset;
			ILOffsetMapping = ilOffsetMapping;
			NativeMethodAddress = nativeMethodAddress;
			NativeMethodOffset = nativeMethodOffset;
			CorCode = corCode ?? throw new ArgumentNullException(nameof(corCode));
		}

		public override DbgCodeLocation Clone() =>
			owner.Create(Module, Token, ILOffset, ILOffsetMapping, NativeMethodAddress, NativeMethodOffset, CorCode.AddRef());

		protected override void CloseCore() => CorCode.Close();

		public override bool Equals(object obj) =>
			obj is DbgDotNetNativeCodeLocationImpl other &&
			CorCode.Object == other.CorCode.Object &&
			Module == other.Module &&
			Token == other.Token &&
			ILOffset == other.ILOffset &&
			ILOffsetMapping == other.ILOffsetMapping &&
			NativeMethodAddress == other.NativeMethodAddress &&
			NativeMethodOffset == other.NativeMethodOffset;

		public override int GetHashCode() => Module.GetHashCode() ^ (int)Token ^ (int)ILOffset ^ (int)ILOffsetMapping ^ NativeMethodAddress.GetHashCode() ^ (int)NativeMethodOffset ^ CorCode.HashCode;
	}
}