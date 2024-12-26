#!/bin/sh

# blocks until kafka is reachable
kafka-topics --bootstrap-server kafka:9092 --list

echo -e 'Creating kafka topics'
kafka-topics --bootstrap-server kafka:9092 --create --if-not-exists --topic user-creation --replication-factor 1 --partitions 1
kafka-topics --bootstrap-server kafka:9092 --create --if-not-exists --topic user-stripe-id-creation --replication-factor 1 --partitions 1
kafka-topics --bootstrap-server kafka:9092 --create --if-not-exists --topic product-creation --replication-factor 1 --partitions 1
kafka-topics --bootstrap-server kafka:9092 --create --if-not-exists --topic product-price-id-creation --replication-factor 1 --partitions 1
kafka-topics --bootstrap-server kafka:9092 --create --if-not-exists --topic order-actions --replication-factor 1 --partitions 1

echo -e 'Successfully created the following topics:'
kafka-topics --bootstrap-server kafka:9092 --list