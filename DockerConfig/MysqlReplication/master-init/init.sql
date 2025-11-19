CREATE USER 'replicator'@'%' IDENTIFIED WITH mysql_native_password BY 'replicatorpass';
GRANT REPLICATION SLAVE ON *.* TO 'replicator'@'%';
FLUSH PRIVILEGES;

CREATE USER IF NOT EXISTS 'master'@'%' IDENTIFIED BY 'master';
GRANT ALL PRIVILEGES ON products.* TO 'master'@'%';
GRANT REPLICATION SLAVE ON *.* TO 'master'@'%';
FLUSH PRIVILEGES;