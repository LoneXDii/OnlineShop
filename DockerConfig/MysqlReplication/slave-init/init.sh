#!/bin/bash
set -e

# Ждем, пока мастер станет доступен
until mysql -uroot -proot -h products-master -e "SELECT 1"; do
  echo "Waiting for master..."
  sleep 5
done

# Получаем информацию о мастере
MASTER_STATUS=$(mysql -uroot -proot -h products-master -e "SHOW MASTER STATUS;" | awk 'NR==2 {print $1, $2}')
MASTER_LOG_FILE=$(echo $MASTER_STATUS | awk '{print $1}')
MASTER_LOG_POS=$(echo $MASTER_STATUS | awk '{print $2}')

# Настройка слейва
mysql -uroot -proot <<EOF
CHANGE MASTER TO
  MASTER_HOST='products-master',
  MASTER_PORT=3306,
  MASTER_USER='replicator',
  MASTER_PASSWORD='replicatorpass',
  MASTER_LOG_FILE='$MASTER_LOG_FILE',
  MASTER_LOG_POS=$MASTER_LOG_POS;
START SLAVE;
EOF