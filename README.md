# sunshine
向日葵远程管理工具



管理向日葵账号，支持直接发起连接（Windows）。



使用PostgreSQL数据库

```sql
CREATE TABLE remote_accounts (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    remark TEXT,
    code VARCHAR(100) NOT NULL,
    password VARCHAR(100),
    type VARCHAR(50) NOT NULL
);

```

