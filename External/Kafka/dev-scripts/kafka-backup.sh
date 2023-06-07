mkdir -p ./Local/KafkaVolume
. ./Local/KafkaVolume
mkdir -p ./kafka-data
mkdir -p ./zookeeper-data
mkdir -p ./logs
docker-compose cp kafka:/kafka-data ./Local/KafkaVolume/kafka-data
docker-compose cp kafka:/zookeeper-data ./Local/KafkaVolume/zookeeper-data
docker-compose cp kafka:/kafka/logs ./Local/KafkaVolume/logs