CREATE TABLE [dbo].[TranCert] (
    [CertId]         UNIQUEIDENTIFIER NOT NULL,
    [PaymentAccount] UNIQUEIDENTIFIER NOT NULL,
    [TranId]         UNIQUEIDENTIFIER NULL,
    [SerialNumber]   VARCHAR (16)     NULL,
    [data1]          VARCHAR (MAX)    NULL,
    [data2]          VARCHAR (MAX)    NULL,
    [CreateTime]     DATETIME         NOT NULL,
    [CreateUser]     BIGINT           NOT NULL,
    CONSTRAINT [PK_TranCert] PRIMARY KEY CLUSTERED ([CertId] ASC)
);

