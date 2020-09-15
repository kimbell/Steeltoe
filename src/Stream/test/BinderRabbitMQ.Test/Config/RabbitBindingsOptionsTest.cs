﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;
using RabbitCore = Steeltoe.Messaging.Rabbit.Core;

namespace Steeltoe.Stream.Binder.Rabbit.Config
{
    public class RabbitBindingsOptionsTest
    {
        [Fact]
        public void InitializeAll_FromDefaultValues()
        {
            var builder = new ConfigurationBuilder();
            var config = builder.Build().GetSection("spring:cloud:stream:rabbit");
            var options = new RabbitBindingsOptions(config);
            options.PostProcess();
            Assert.NotNull(options.Default);
            Assert.NotNull(options.Default.Consumer);
            Assert.NotNull(options.Default.Producer);

            // TODO: Verify all default property values
            Assert.Equal(1, options.Default.Consumer.Prefetch);

            Assert.Equal(100, options.Default.Producer.BatchSize);
        }

        [Fact]
        public void InitializeAll_FromConfigValues()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "spring:cloud:stream:rabbit:default:consumer:autoBindDlq", "true" },
                { "spring:cloud:stream:rabbit:default:consumer:dlqMaxLength", "10000" },
                { "spring:cloud:stream:rabbit:default:producer:autoBindDlq", "true" },
                { "spring:cloud:stream:rabbit:default:producer:dlqMaxLength", "10000" },

                // { "spring:cloud:stream:rabbit:bindings:input:consumer:autoBindDlq", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:bindingRoutingKey", "bindingRoutingKey" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:bindingRoutingKeyDelimiter", "bindingRoutingKeyDelimiter" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:bindQueue", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:consumerTagPrefix", "consumerTagPrefix" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:deadLetterQueueName", "deadLetterQueueName" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:deadLetterExchange", "deadLetterExchange" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:deadLetterExchangeType", "deadLetterExchangeType" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:deadLetterRoutingKey", "deadLetterRoutingKey" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:declareDlx", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:declareExchange", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:delayedExchange", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqBindingArguments:foo", "bar" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqBindingArguments:bar", "foo" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqDeadLetterExchange", "dlqDeadLetterExchange" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqDeadLetterRoutingKey", "dlqDeadLetterRoutingKey" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqExpires", "1000" },

                // { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqLazy", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqMaxLength", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqMaxLengthBytes", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqMaxPriority", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqOverflowBehavior", "dlqOverflowBehavior" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqSingleActiveConsumer", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqTtl", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:durableSubscription", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:exchangeAutoDelete", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:exchangeDurable", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:exchangeType", "exchangeType" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:exclusive", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:expires", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:failedDeclarationRetryInterval", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:frameMaxHeadroom", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:headerPatterns:0", "headerPatterns" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:lazy", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:maxConcurrency", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:maxLength", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:maxLengthBytes", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:maxPriority", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:missingQueuesFatal", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:overflowBehavior", "overflowBehavior" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:prefetch", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:prefix", "prefix" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:queueBindingArguments:foo", "bar" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:queueBindingArguments:bar", "foo" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:queueDeclarationRetries", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:queueNameGroupOnly", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:recoveryInterval", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:requeueRejected", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:republishDeliveryMode", "NON_PERSISTENT" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:republishToDlq", "false" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:singleActiveConsumer", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:transacted", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:txSize", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:ttl", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:quorum:deliveryLimit", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:quorum:enabled", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:quorum:initialQuorumSize", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqQuorum:deliveryLimit", "1000" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqQuorum:enabled", "true" },
                { "spring:cloud:stream:rabbit:bindings:input:consumer:dlqQuorum:initialQuorumSize", "1000" },

                // { "spring:cloud:stream:rabbit:bindings:output:producer:autoBindDlq", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:batchingEnabled", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:batchSize", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:batchBufferLimit", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:batchTimeout", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:bindingRoutingKey", "bindingRoutingKey" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:bindingRoutingKeyDelimiter", "bindingRoutingKeyDelimiter" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:bindQueue", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:compress", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:confirmAckChannel", "confirmAckChannel" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:deadLetterQueueName", "deadLetterQueueName" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:deadLetterExchange", "deadLetterExchange" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:deadLetterExchangeType", "deadLetterExchangeType" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:deadLetterRoutingKey", "deadLetterRoutingKey" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:declareDlx", "false" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:declareExchange", "false" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:delayExpression", "delayExpression" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:delayedExchange", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:deliveryMode", "NON_PERSISTENT" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqBindingArguments:foo", "bar" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqBindingArguments:bar", "foo" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqDeadLetterExchange", "dlqDeadLetterExchange" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqDeadLetterRoutingKey", "dlqDeadLetterRoutingKey" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqExpires", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqLazy", "true" },

                // { "spring:cloud:stream:rabbit:bindings:output:producer:dlqMaxLength", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqMaxLengthBytes", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqMaxPriority", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqSingleActiveConsumer", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqTtl", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:durableSubscription", "false" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:exchangeAutoDelete", "false" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:exchangeDurable", "false" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:exchangeType", "exchangeType" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:exclusive", "false" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:expires", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:failedDeclarationRetryInterval", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:frameMaxHeadroom", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:headerPatterns:0", "headerPatterns0" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:headerPatterns:1", "headerPatterns1" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:lazy", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:maxConcurrency", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:maxLength", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:maxLengthBytes", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:maxPriority", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:missingQueuesFatal", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:overflowBehavior", "overflowBehavior" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:prefetch", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:prefix", "prefix" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:queueBindingArguments:foo", "bar" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:queueBindingArguments:bar", "foo" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:queueDeclarationRetries", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:queueNameGroupOnly", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:routingKeyExpression", "routingKeyExpression" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:singleActiveConsumer", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:transacted", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:txSize", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:ttl", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:quorum:deliveryLimit", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:quorum:enabled", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:quorum:initialQuorumSize", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqQuorum:deliveryLimit", "1000" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqQuorum:enabled", "true" },
                { "spring:cloud:stream:rabbit:bindings:output:producer:dlqQuorum:initialQuorumSize", "1000" },
                { "spring:cloud:stream:rabbit:binder:adminAddresses:0", "adminAddresses0" },
                { "spring:cloud:stream:rabbit:binder:adminAddresses:1", "adminAddresses1" },
                { "spring:cloud:stream:rabbit:binder:nodes:0", "nodes0" },
                { "spring:cloud:stream:rabbit:binder:nodes:1", "nodes1" },
                { "spring:cloud:stream:rabbit:binder:compressionLevel", "NoCompression" },
                { "spring:cloud:stream:rabbit:binder:connectionNamePrefix", "connectionNamePrefix" },
            });

            var config = builder.Build().GetSection("spring:cloud:stream:rabbit");
            var options = new RabbitBindingsOptions(config);
            options.PostProcess();
            Assert.NotNull(options.Default);
            Assert.NotNull(options.Default.Consumer);
            Assert.NotNull(options.Default.Producer);

            Assert.True(options.Default.Consumer.AutoBindDlq);
            Assert.Equal(10000, options.Default.Consumer.DlqMaxLength);
            Assert.True(options.Default.Producer.AutoBindDlq);
            Assert.Equal(10000, options.Default.Producer.DlqMaxLength);

            var inputBinding = options.GetRabbitConsumerOptions("input");
            Assert.NotNull(inputBinding);
            Assert.NotSame(options.Default.Consumer, inputBinding);
            var outputBinding = options.GetRabbitProducerOptions("output");
            Assert.NotNull(outputBinding);
            Assert.NotSame(options.Default.Producer, outputBinding);

            // TODO: Verify all property values are set properly

            // Dictionary type propery
            Assert.NotNull(outputBinding.QueueBindingArguments);
            Assert.Equal("bar", outputBinding.QueueBindingArguments["foo"]);
            Assert.Equal("foo", outputBinding.QueueBindingArguments["bar"]);

            // List type property
            Assert.NotNull(outputBinding.HeaderPatterns);
            Assert.Equal("headerPatterns0", outputBinding.HeaderPatterns[0]);
            Assert.Equal("headerPatterns1", outputBinding.HeaderPatterns[1]);

            // Enum type property
            Assert.Equal(RabbitCore.MessageDeliveryMode.NON_PERSISTENT, outputBinding.DeliveryMode);
        }
    }
}
