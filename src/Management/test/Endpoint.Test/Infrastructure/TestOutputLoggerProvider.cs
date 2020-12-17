// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Steeltoe.Management.Endpoint.Test.Infrastructure
{
    /// <summary>
    /// This provider allows us to capture log messages and show them for each xUnit test
    /// </summary>
    internal class TestOutputLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;
        private readonly ConcurrentDictionary<string, TestOutputLogger> _loggers = new ConcurrentDictionary<string, TestOutputLogger>();

        /// <summary>
        /// Gets all the entries that have been logged
        /// </summary>
        public List<LogEntry> Entries { get; } = new List<LogEntry>();

        /// <summary>
        /// Gets or sets a value indicating whether logs should be keept for further analysis
        /// </summary>
        public bool EnableLogCapture { get; set; }

        public TestOutputLoggerProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, _ => new TestOutputLogger(_output, categoryName, Entries, EnableLogCapture));
        }

        public void Dispose()
        {
            Entries.Clear();
            _loggers.Clear();
        }

        public void VerifyLog(LogLevel logLevel, string? message = null, string? category = null, int? count = null)
        {
            lock (Entries)
            {
                if (!EnableLogCapture)
                {
                    throw new InvalidOperationException($"In order to check logs, set {nameof(EnableLogCapture)} to true");
                }

                var byLogLevel = Entries.Where(l => l.LogLevel == logLevel);

                if (string.IsNullOrEmpty(message) == false)
                {
                    byLogLevel = byLogLevel.Where(l => l.Message.Contains(message));
                }

                if (string.IsNullOrEmpty(category) == false)
                {
                    byLogLevel = byLogLevel.Where(l => l.Category.Contains(category));
                }

                if (count == null)
                {
                    Assert.NotNull(byLogLevel.FirstOrDefault());
                }
                else
                {
                    Assert.Equal(count, byLogLevel.Count());
                }
            }
        }
    }

    internal class TestOutputLogger : ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly string _category;
        private readonly List<LogEntry> _entries;
        private readonly bool _enableLogCapture;

        public TestOutputLogger(ITestOutputHelper output, string category, List<LogEntry> entries, bool enableLogCapture)
        {
            _output = output;
            _category = category;
            _entries = entries;
            _enableLogCapture = enableLogCapture;
        }

        public IDisposable? BeginScope<TState>(TState state)
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            lock (_entries)
            {
                var formattedMessage = formatter(state, exception);
                if (_enableLogCapture)
                {
                    _entries.Add(new LogEntry
                    {
                        Category = _category,
                        LogLevel = logLevel,
                        Message = formattedMessage
                    });
                }

                // show in xUnit logs
                try
                {
                    _output?.WriteLine(formattedMessage);
                    if (exception != null)
                    {
                        _output?.WriteLine(exception.Message);
                        _output?.WriteLine(exception.StackTrace);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    internal class LogEntry
    {
        [NotNull]
        [DisallowNull]
        public string? Category { get; set; }

        public LogLevel LogLevel { get; set; }

        [NotNull]
        [DisallowNull]
        public string? Message { get; set; }
    }
}
