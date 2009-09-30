﻿//-----------------------------------------------------------------------
// <copyright file="jet_threadstats.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.Isam.Esent.Interop.Vista
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Contains cumulative statistics on the work performed by the database
    /// engine on the current thread. This information is returned via
    /// <see cref="VistaApi.JetGetThreadStats"/>.
    /// </summary>
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.NamingRules",
        "SA1300:ElementMustBeginWithUpperCaseLetter",
        Justification = "This should match the unmanaged API, which isn't capitalized.")]
    public struct JET_THREADSTATS
    {
        /// <summary>
        /// Gets the total number of database pages visited by the database
        /// engine on the current thread.
        /// </summary>
        public int cPageReferenced { get; internal set; }

        /// <summary>
        /// Gets the total number of database pages fetched from disk by the
        /// database engine on the current thread.
        /// </summary>
        public int cPageRead { get; internal set; }

        /// <summary>
        /// Gets the total number of database pages prefetched from disk by
        /// the database engine on the current thread.
        /// </summary>
        public int cPagePreread { get; internal set; }

        /// <summary>
        /// Gets the total number of database pages, with no unwritten changes,
        /// that have been modified by the database engine on the current thread.
        /// </summary>
        public int cPageDirtied { get; internal set; }

        /// <summary>
        /// Gets the total number of database pages, with unwritten changes, that
        /// have been modified by the database engine on the current thread.
        /// </summary>
        public int cPageRedirtied { get; internal set; }

        /// <summary>
        /// Gets the total number of transaction log records that have been
        /// generated by the database engine on the current thread.
        /// </summary>
        public int cLogRecord { get; internal set; }

        /// <summary>
        /// Gets the total size in bytes of transaction log records that
        /// have been generated by the database engine on the current thread.
        /// </summary>
        public int cbLogRecord { get; internal set; }

        /// <summary>
        /// Add the stats in two JET_THREADSTATS structures.
        /// </summary>
        /// <param name="t1">The first JET_THREADSTATS.</param>
        /// <param name="t2">The second JET_THREADSTATS.</param>
        /// <returns>A JET_THREADSTATS containing the result of adding the stats in t1 and t2.</returns>
        public static JET_THREADSTATS operator +(JET_THREADSTATS t1, JET_THREADSTATS t2)
        {
            return new JET_THREADSTATS
            {
                cPageReferenced = t1.cPageReferenced + t2.cPageReferenced,
                cPageRead = t1.cPageRead + t2.cPageRead,
                cPagePreread = t1.cPagePreread + t2.cPagePreread,
                cPageDirtied = t1.cPageDirtied + t2.cPageDirtied,
                cPageRedirtied = t1.cPageRedirtied + t2.cPageRedirtied,
                cLogRecord = t1.cLogRecord + t2.cLogRecord,
                cbLogRecord = t1.cbLogRecord + t2.cbLogRecord,
            };
        }

        /// <summary>
        /// Calculate the differeence in stats between two JET_THREADSTATS structures.
        /// </summary>
        /// <param name="t1">The first JET_THREADSTATS.</param>
        /// <param name="t2">The second JET_THREADSTATS.</param>
        /// <returns>A JET_THREADSTATS containing the difference in stats between t1 and t2.</returns>
        public static JET_THREADSTATS operator -(JET_THREADSTATS t1, JET_THREADSTATS t2)
        {
            return new JET_THREADSTATS
            {
                cPageReferenced = t1.cPageReferenced - t2.cPageReferenced,
                cPageRead = t1.cPageRead - t2.cPageRead,
                cPagePreread = t1.cPagePreread - t2.cPagePreread,
                cPageDirtied = t1.cPageDirtied - t2.cPageDirtied,
                cPageRedirtied = t1.cPageRedirtied - t2.cPageRedirtied,
                cLogRecord = t1.cLogRecord - t2.cLogRecord,
                cbLogRecord = t1.cbLogRecord - t2.cbLogRecord,
            };
        }

        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A string representation of this object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} page reference{1}, ", this.cPageReferenced, GetPluralS(this.cPageReferenced));
            sb.AppendFormat("{0} page{1} read, ", this.cPageRead, GetPluralS(this.cPageRead));
            sb.AppendFormat("{0} page{1} preread, ", this.cPagePreread, GetPluralS(this.cPagePreread));
            sb.AppendFormat("{0} page{1} dirtied, ", this.cPageDirtied, GetPluralS(this.cPageDirtied));
            sb.AppendFormat("{0} page{1} redirtied, ", this.cPageRedirtied, GetPluralS(this.cPageRedirtied));
            sb.AppendFormat("{0} log record{1}, ", this.cLogRecord, GetPluralS(this.cLogRecord));
            sb.AppendFormat("{0} byte{1} logged", this.cbLogRecord, GetPluralS(this.cbLogRecord));
            return sb.ToString();
        }

        /// <summary>
        /// Sets the fields of the object from a NATIVE_THREADSTATS struct.
        /// </summary>
        /// <param name="value">
        /// The native threadstats to set the values from.
        /// </param>
        internal void SetFromNativeThreadstats(NATIVE_THREADSTATS value)
        {
            this.cPageReferenced = checked((int)value.cPageReferenced);
            this.cPageRead = checked((int)value.cPageRead);
            this.cPagePreread = checked((int)value.cPagePreread);
            this.cPageDirtied = checked((int)value.cPageDirtied);
            this.cPageRedirtied = checked((int)value.cPageRedirtied);
            this.cLogRecord = checked((int)value.cLogRecord);
            this.cbLogRecord = checked((int)value.cbLogRecord);
        }

        /// <summary>
        /// Get the plural suffix ('s') for the given number.
        /// </summary>
        /// <param name="n">The number.</param>
        /// <returns>The letter 's' if n is greater than 1.</returns>
        private static string GetPluralS(int n)
        {
            return n == 1 ? String.Empty : "s";
        }
    }

    /// <summary>
    /// The native version of the JET_THREADSTATS structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Internal interop struct only.")]
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.NamingRules",
        "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
        Justification = "This should match the unmanaged API, which isn't capitalized.")]
    internal struct NATIVE_THREADSTATS
    {
        public static readonly int Size = Marshal.SizeOf(typeof(NATIVE_THREADSTATS));

        public uint cbStruct;
        public uint cPageReferenced;
        public uint cPageRead;
        public uint cPagePreread;
        public uint cPageDirtied;
        public uint cPageRedirtied;
        public uint cLogRecord;
        public uint cbLogRecord;
    }
}