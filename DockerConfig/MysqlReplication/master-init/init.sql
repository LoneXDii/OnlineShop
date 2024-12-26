CREATE USER 'replicator'@'%' IDENTIFIED WITH mysql_native_password BY 'replicatorpass';
GRANT REPLICATION SLAVE ON *.* TO 'replicator'@'%';
FLUSH PRIVILEGES;
SHOW MASTER STATUS;