#Getting Started

First run the containers:

```bash
docker-compose up
```

Then enter the master database:

```bash
docker exec -it products-master bash
```

Enter to MySQL:

```bash
mysql -uroot -proot
```

Run the following commands:

```bash
CREATE USER 'replicator'@'%' IDENTIFIED BY 'replicatorpass';
GRANT REPLICATION SLAVE ON *.* TO 'replicator'@'%';
FLUSH PRIVILEGES;
```

Then run:

```bash
SHOW MASTER STATUS;
```

Then go to slave databse:

```bash
docker exec -it products-slave bash
mysql -uroot -proot
```

Then run:
```bash
CHANGE MASTER TO
MASTER_HOST='products-master',
MASTER_PORT=3306,
MASTER_USER='replicator',
MASTER_PASSWORD='replicatorpass',
MASTER_LOG_FILE='mysql-bin.000003',
MASTER_LOG_POS=868,
GET_MASTER_PUBLIC_KEY=1;
```
```bash
START SLAVE;
```
MASTER_LOG_FILE and MASTER_LOG_POS must be the same as you get after executing SHOW MASTER STATUS on master db

Now your databases are configured for replication