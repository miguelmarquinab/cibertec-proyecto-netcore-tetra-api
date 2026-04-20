
-- =========================================================
-- 01. LIMPIEZA
-- =========================================================
IF OBJECT_ID('dbo.asignacionRadios', 'U') IS NOT NULL DROP TABLE dbo.asignacionRadios;
IF OBJECT_ID('dbo.detalleContratos', 'U') IS NOT NULL DROP TABLE dbo.detalleContratos;
IF OBJECT_ID('dbo.contratos', 'U') IS NOT NULL DROP TABLE dbo.contratos;
IF OBJECT_ID('dbo.radios', 'U') IS NOT NULL DROP TABLE dbo.radios;
IF OBJECT_ID('dbo.modeloRadio', 'U') IS NOT NULL DROP TABLE dbo.modeloRadio;
IF OBJECT_ID('dbo.estadoradio', 'U') IS NOT NULL DROP TABLE dbo.estadoradio;
IF OBJECT_ID('dbo.proveedores', 'U') IS NOT NULL DROP TABLE dbo.proveedores;
IF OBJECT_ID('dbo.clientes', 'U') IS NOT NULL DROP TABLE dbo.clientes;
IF OBJECT_ID('dbo.usuarios', 'U') IS NOT NULL DROP TABLE dbo.usuarios;
IF OBJECT_ID('dbo.roles', 'U') IS NOT NULL DROP TABLE dbo.roles;
GO

-- =========================================================
-- 02. TABLAS MAESTRAS
-- =========================================================
CREATE TABLE dbo.roles
(
    rol_id      INT IDENTITY(1,1) PRIMARY KEY,
    rol_nombre  VARCHAR(100) NULL,
    rol_estado  VARCHAR(100) NULL,
    rol_activo  BIT NULL
);
GO

CREATE TABLE dbo.usuarios
(
    usa_id               INT IDENTITY(1,1) PRIMARY KEY,
    nombre               VARCHAR(100) NULL,
    clave                VARCHAR(100) NULL,
    usa_nombres          VARCHAR(100) NULL,
    usa_apellidopaterno  VARCHAR(100) NULL,
    usa_apellidomaterno  VARCHAR(100) NULL,
    usa_fechanacimiento  VARCHAR(100) NULL,
    usa_genero           VARCHAR(100) NULL,
    usa_estado           VARCHAR(100) NULL,
    usa_activo           VARCHAR(100) NULL,
    usa_filafecha        DATETIME NULL,
    usa_filaoriginal     BIT NULL,
    usa_filaeliminada    BIT NULL,
    rol                  INT NULL,
    CONSTRAINT FK_usuarios_roles
        FOREIGN KEY (rol) REFERENCES dbo.roles(rol_id)
);
GO

CREATE TABLE dbo.estadoradio
(
    esr_id             INT IDENTITY(1,1) PRIMARY KEY,
    esr_descripcion    VARCHAR(100) NULL,
    esr_sigla          VARCHAR(100) NULL,
    esr_activo         BIT NULL,
    esr_filafecha      DATETIME NULL,
    esr_filaoriginal   BIT NULL,
    esr_filaeliminada  BIT NULL,
    usa_id             INT NULL,
    CONSTRAINT FK_estadoradio_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

CREATE TABLE dbo.modeloRadio
(
    mod_id             INT IDENTITY(1,1) PRIMARY KEY,
    mod_descripcion    VARCHAR(100) NULL,
    mod_codigo         VARCHAR(100) NULL,
    mod_filaFecha      DATETIME2 NULL CONSTRAINT DF_modeloRadio_mod_filaFecha DEFAULT SYSDATETIME(),
    mod_filaOriginal   BIT NULL CONSTRAINT DF_modeloRadio_mod_filaOriginal DEFAULT 1,
    mod_filaEliminada  BIT NULL CONSTRAINT DF_modeloRadio_mod_filaEliminada DEFAULT 0,
    usa_id             INT NULL,
    CONSTRAINT FK_modeloRadio_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

CREATE TABLE dbo.proveedores
(
    pve_id            INT IDENTITY(1,1) PRIMARY KEY,
    pve_codigo        VARCHAR(50) NOT NULL,
    pve_razon_social  VARCHAR(255) NOT NULL,
    pve_pais          VARCHAR(100) NULL,
    pve_ruc           VARCHAR(20) NULL,
    usa_id            INT NOT NULL,
    CONSTRAINT UQ_proveedores_codigo UNIQUE (pve_codigo),
    CONSTRAINT FK_proveedores_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

-- =========================================================
-- 03. CLIENTES Y CONTRATOS
-- =========================================================
CREATE TABLE dbo.clientes
(
    cli_id             INT IDENTITY(1,1) PRIMARY KEY,
    cli_codigo         VARCHAR(50) NOT NULL,
    cli_razonSocial    VARCHAR(255) NOT NULL,
    cli_tipoDocumento  VARCHAR(20) NULL,
    cli_nroDocumento   VARCHAR(20) NOT NULL,
    cli_direccion      VARCHAR(MAX) NULL,
    cli_telefono       VARCHAR(50) NULL,
    cli_email          VARCHAR(100) NULL,
    cli_contacto       VARCHAR(255) NULL,
    cli_activo         BIT NULL CONSTRAINT DF_clientes_cli_activo DEFAULT 1,
    cli_filaFecha      DATETIME2 NULL CONSTRAINT DF_clientes_cli_filaFecha DEFAULT SYSDATETIME(),
    cli_filaOriginal   BIT NULL CONSTRAINT DF_clientes_cli_filaOriginal DEFAULT 1,
    cli_filaEliminada  BIT NULL CONSTRAINT DF_clientes_cli_filaEliminada DEFAULT 0,
    usa_id             INT NOT NULL,
    CONSTRAINT UQ_clientes_cli_codigo UNIQUE (cli_codigo),
    CONSTRAINT FK_clientes_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

CREATE TABLE dbo.contratos
(
    con_id             INT IDENTITY(1,1) PRIMARY KEY,
    cli_id             INT NOT NULL,
    con_numero         VARCHAR(100) NOT NULL,
    con_fechaInicio    DATE NOT NULL,
    con_fechaFin       DATE NOT NULL,
    con_estado         VARCHAR(50) NULL CONSTRAINT DF_contratos_con_estado DEFAULT 'ACTIVO',
    con_tipoContrato   VARCHAR(50) NULL,
    con_valorTotal     DECIMAL(12,2) NULL,
    con_valorMensual   DECIMAL(10,2) NULL,
    con_observaciones  VARCHAR(MAX) NULL,
    con_filaEliminada  BIT NULL CONSTRAINT DF_contratos_con_filaEliminada DEFAULT 0,
    usa_id             INT NULL CONSTRAINT DF_contratos_usa_id DEFAULT 1,
    CONSTRAINT UQ_contratos_con_numero UNIQUE (con_numero),
    CONSTRAINT FK_contratos_clientes
        FOREIGN KEY (cli_id) REFERENCES dbo.clientes(cli_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_contratos_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

CREATE TABLE dbo.detalleContratos
(
    dec_id              INT IDENTITY(1,1) PRIMARY KEY,
    con_id              INT NOT NULL,
    mod_id              INT NOT NULL,
    dec_cantidad        INT NOT NULL,
    dec_precioUnitario  DECIMAL(10,2) NOT NULL,
    dec_subtotal        DECIMAL(12,2) NOT NULL,
    dec_filaEliminada   BIT NULL CONSTRAINT DF_detalleContratos_dec_filaEliminada DEFAULT 0,
    usa_id              INT NULL CONSTRAINT DF_detalleContratos_usa_id DEFAULT 1,
    CONSTRAINT FK_detalleContratos_contratos
        FOREIGN KEY (con_id) REFERENCES dbo.contratos(con_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_detalleContratos_modeloRadio
        FOREIGN KEY (mod_id) REFERENCES dbo.modeloRadio(mod_id),
    CONSTRAINT FK_detalleContratos_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

-- =========================================================
-- 04. RADIOS
-- =========================================================
CREATE TABLE dbo.radios
(
    rad_id             INT IDENTITY(1,1) PRIMARY KEY,
    mod_id             INT NULL,
    esr_id             INT NULL,
    serie              VARCHAR(100) NULL,
    fecha_ingreso      DATETIME NULL,
    rad_activo         BIT NULL,
    rad_filafecha      DATETIME NULL,
    rad_filaoriginal   BIT NULL,
    rad_filaeliminado  BIT NULL,
    usa_id             INT NULL,
    CONSTRAINT FK_radios_modeloRadio
        FOREIGN KEY (mod_id) REFERENCES dbo.modeloRadio(mod_id),
    CONSTRAINT FK_radios_estadoradio
        FOREIGN KEY (esr_id) REFERENCES dbo.estadoradio(esr_id),
    CONSTRAINT FK_radios_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO

CREATE TABLE dbo.asignacionRadios
(
    asr_id                         INT PRIMARY KEY,
    dec_id                         INT NOT NULL,
    rad_id                         INT NOT NULL,
    asr_fechaAsignacion            DATE NOT NULL,
    asr_fechaDevolucion            DATE NULL,
    asr_estado                     VARCHAR(50) NULL CONSTRAINT DF_asignacionRadios_asr_estado DEFAULT 'ASIGNADO',
    asr_observacionesAsignacion    VARCHAR(MAX) NULL,
    asr_observacionesDevolucion    VARCHAR(MAX) NULL,
    asr_filaFecha                  DATETIME2 NULL CONSTRAINT DF_asignacionRadios_asr_filaFecha DEFAULT SYSDATETIME(),
    asr_filaOriginal               BIT NULL CONSTRAINT DF_asignacionRadios_asr_filaOriginal DEFAULT 1,
    asr_filaEliminada              BIT NULL CONSTRAINT DF_asignacionRadios_asr_filaEliminada DEFAULT 0,
    usa_id                         INT NOT NULL,
    CONSTRAINT FK_asignacionRadios_detalleContratos
        FOREIGN KEY (dec_id) REFERENCES dbo.detalleContratos(dec_id),
    CONSTRAINT FK_asignacionRadios_radios
        FOREIGN KEY (rad_id) REFERENCES dbo.radios(rad_id),
    CONSTRAINT FK_asignacionRadios_usuarios
        FOREIGN KEY (usa_id) REFERENCES dbo.usuarios(usa_id)
);
GO


-------------------------------
--*****************************
--------------------------------

-- =========================================================
-- 05. DROP PROCEDURES
-- =========================================================
IF OBJECT_ID('dbo.usp_Buscar_Contratos', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Buscar_Contratos;
IF OBJECT_ID('dbo.usp_Buscar_ModeloRadio', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Buscar_ModeloRadio;
IF OBJECT_ID('dbo.usp_Buscar_Radio', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Buscar_Radio;
IF OBJECT_ID('dbo.usp_Buscar_Usuario', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_Buscar_Usuario;
IF OBJECT_ID('dbo.usp_radio_eliminar', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_radio_eliminar;
IF OBJECT_ID('dbo.usp_radio_estados', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_radio_estados;
IF OBJECT_ID('dbo.usp_radio_guardar', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_radio_guardar;
IF OBJECT_ID('dbo.usp_radio_modelos', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_radio_modelos;
IF OBJECT_ID('dbo.usp_radio_obtener', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_radio_obtener;
GO

-- =========================================================
-- 06. PROCEDURES
-- =========================================================

CREATE PROCEDURE dbo.usp_Buscar_Contratos
    @cliente_id   INT,
    @estado       VARCHAR(100),
    @fecha_inicio VARCHAR(10),
    @fecha_fin    VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.con_id,
        cl.cli_id,
        c.con_numero,
        c.con_fechaInicio,
        c.con_fechaFin,
        c.con_valorMensual,
        c.con_estado,
        ISNULL(SUM(dc.dec_cantidad), 0) AS nro_radios,
        cl.cli_razonSocial
    FROM dbo.contratos c
    INNER JOIN dbo.clientes cl
        ON c.cli_id = cl.cli_id
    LEFT JOIN dbo.detalleContratos dc
        ON c.con_id = dc.con_id
    WHERE c.con_estado LIKE '%' + ISNULL(@estado, '') + '%'
      AND (@cliente_id = 0 OR c.cli_id = @cliente_id)
      AND (ISNULL(@fecha_inicio, '') = '' OR c.con_fechaInicio = TRY_CONVERT(DATE, @fecha_inicio, 23))
      AND (ISNULL(@fecha_fin, '') = '' OR c.con_fechaFin = TRY_CONVERT(DATE, @fecha_fin, 23))
    GROUP BY
        c.con_id,
        cl.cli_id,
        c.con_numero,
        c.con_fechaInicio,
        c.con_fechaFin,
        c.con_valorMensual,
        c.con_estado,
        cl.cli_razonSocial;
END;
GO




CREATE PROCEDURE dbo.usp_Buscar_ModeloRadio
    @texto VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mod_id,
        mod_descripcion,
        mod_codigo
    FROM dbo.modeloRadio
    WHERE mod_descripcion LIKE '%' + ISNULL(@texto, '') + '%'
       OR mod_codigo      LIKE '%' + ISNULL(@texto, '') + '%';
END;
GO




CREATE PROCEDURE dbo.usp_Buscar_Radio
    @p_texto VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.rad_id,
        mr.mod_codigo,
        mr.mod_descripcion AS modelo,
        e.esr_descripcion AS estado,
        r.serie,
        r.fecha_ingreso,
        r.rad_activo
    FROM dbo.radios r
    INNER JOIN dbo.modeloRadio mr
        ON mr.mod_id = r.mod_id
    INNER JOIN dbo.estadoradio e
        ON e.esr_id = r.esr_id
    WHERE (
        @p_texto IS NULL OR @p_texto = ''
        OR r.serie LIKE '%' + @p_texto + '%'
        OR mr.mod_descripcion LIKE '%' + @p_texto + '%'
        OR e.esr_descripcion LIKE '%' + @p_texto + '%'
    );
END;
GO



CREATE PROCEDURE dbo.usp_Buscar_Usuario
    @p_nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.nombre,
        r.rol_nombre
    FROM dbo.usuarios u
    INNER JOIN dbo.roles r
        ON r.rol_id = u.rol
    WHERE u.nombre = @p_nombre;
END;
GO



CREATE PROCEDURE dbo.usp_radio_eliminar
    @p_rad_id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.radios
    WHERE rad_id = @p_rad_id;
END;
GO



CREATE PROCEDURE dbo.usp_radio_estados
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        esr_id,
        esr_descripcion
    FROM dbo.estadoradio
    ORDER BY esr_descripcion;
END;
GO



CREATE PROCEDURE dbo.usp_radio_guardar
    @p_rad_id         INT,
    @p_mod_id         INT,
    @p_esr_id         INT,
    @p_serie          VARCHAR(50),
    @p_fecha_ingreso  DATETIME,
    @p_activo         BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(@p_rad_id, 0) = 0
    BEGIN
        INSERT INTO dbo.radios
        (
            mod_id,
            esr_id,
            serie,
            fecha_ingreso,
            rad_activo
        )
        VALUES
        (
            @p_mod_id,
            @p_esr_id,
            @p_serie,
            @p_fecha_ingreso,
            @p_activo
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT) AS nuevo_id;
    END
    ELSE
    BEGIN
        UPDATE dbo.radios
        SET mod_id = @p_mod_id,
            esr_id = @p_esr_id,
            serie = @p_serie,
            fecha_ingreso = @p_fecha_ingreso,
            rad_activo = @p_activo
        WHERE rad_id = @p_rad_id;

        SELECT @p_rad_id AS nuevo_id;
    END
END;
GO

CREATE PROCEDURE dbo.usp_radio_modelos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mod_id,
        mod_codigo,
        mod_descripcion
    FROM dbo.modeloRadio
    ORDER BY mod_codigo;
END;
GO

CREATE PROCEDURE dbo.usp_radio_obtener
    @p_rad_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.rad_id AS radio_id,
        r.mod_id AS mod_id,
        r.esr_id AS esr_id,
        r.serie AS serie,
        CONVERT(VARCHAR(19), r.fecha_ingreso, 120) AS fecha_ingreso,
        r.rad_activo AS rad_activo,
        mr.mod_codigo AS mod_codigo,
        mr.mod_descripcion AS modelo,
        e.esr_descripcion AS estado
    FROM dbo.radios r
    LEFT JOIN dbo.modeloRadio mr
        ON mr.mod_id = r.mod_id
    LEFT JOIN dbo.estadoradio e
        ON e.esr_id = r.esr_id
    WHERE r.rad_id = @p_rad_id;
END;
GO

--##############################################
--##############################################

-- =========================================================
-- 07. DATOS DE PRUEBA
-- =========================================================
INSERT INTO dbo.roles (rol_nombre, rol_estado, rol_activo)
VALUES
('ADMIN', 'ACTIVO', 1),
('OPERADOR', 'ACTIVO', 1);

INSERT INTO dbo.usuarios
(
    nombre, clave, usa_nombres, usa_apellidopaterno, usa_apellidomaterno,
    usa_fechanacimiento, usa_genero, usa_estado, usa_activo,
    usa_filafecha, usa_filaoriginal, usa_filaeliminada, rol
)
VALUES
('admin', '123456', 'Miguel', 'Marquina', 'Lopez', '1990-01-01', 'M', 'ACTIVO', 'SI', GETDATE(), 1, 0, 1),
('operador1', '123456', 'Ana', 'Perez', 'Gomez', '1995-05-10', 'F', 'ACTIVO', 'SI', GETDATE(), 1, 0, 2);

INSERT INTO dbo.estadoradio
(
    esr_descripcion, esr_sigla, esr_activo, esr_filafecha,
    esr_filaoriginal, esr_filaeliminada, usa_id
)
VALUES
('DISPONIBLE', 'DIS', 1, GETDATE(), 1, 0, 1),
('ASIGNADO', 'ASI', 1, GETDATE(), 1, 0, 1),
('MANTENIMIENTO', 'MAN', 1, GETDATE(), 1, 0, 1);

INSERT INTO dbo.modeloRadio
(
    mod_descripcion, mod_codigo, usa_id
)
VALUES
('Motorola DEP450', 'MOD-DEP450', 1),
('Kenwood NX-1200', 'MOD-NX1200', 1);

INSERT INTO dbo.clientes
(
    cli_codigo, cli_razonSocial, cli_tipoDocumento, cli_nroDocumento,
    cli_direccion, cli_telefono, cli_email, cli_contacto, usa_id
)
VALUES
('CLI-001', 'Empresa Uno SAC', 'RUC', '20111111111', 'Av. Lima 123', '999111222', 'contacto@empresauno.com', 'Carlos Ruiz', 1),
('CLI-002', 'Servicios Dos EIRL', 'RUC', '20222222222', 'Jr. Peru 456', '988777666', 'ventas@serviciosdos.com', 'Lucia Torres', 1);

INSERT INTO dbo.contratos
(
    cli_id, con_numero, con_fechaInicio, con_fechaFin, con_estado,
    con_tipoContrato, con_valorTotal, con_valorMensual, con_observaciones, usa_id
)
VALUES
(1, 'CON-2026-001', '2026-01-01', '2026-12-31', 'ACTIVO', 'ANUAL', 12000.00, 1000.00, 'Contrato anual cliente 1', 1),
(2, 'CON-2026-002', '2026-02-01', '2026-08-31', 'ACTIVO', 'SEMESTRAL', 6000.00, 1000.00, 'Contrato semestral cliente 2', 1);

INSERT INTO dbo.detalleContratos
(
    con_id, mod_id, dec_cantidad, dec_precioUnitario, dec_subtotal, usa_id
)
VALUES
(1, 1, 5, 200.00, 1000.00, 1),
(1, 2, 3, 250.00, 750.00, 1),
(2, 1, 2, 210.00, 420.00, 1);

INSERT INTO dbo.radios
(
    mod_id, esr_id, serie, fecha_ingreso, rad_activo,
    rad_filafecha, rad_filaoriginal, rad_filaeliminado, usa_id
)
VALUES
(1, 1, 'SERIE-0001', GETDATE(), 1, GETDATE(), 1, 0, 1),
(1, 2, 'SERIE-0002', GETDATE(), 1, GETDATE(), 1, 0, 1),
(2, 1, 'SERIE-0003', GETDATE(), 1, GETDATE(), 1, 0, 1);

INSERT INTO dbo.proveedores
(
    pve_codigo, pve_razon_social, pve_pais, pve_ruc, usa_id
)
VALUES
('PVE-001', 'Proveedor Radios SAC', 'Peru', '20555555555', 1);

INSERT INTO dbo.asignacionRadios
(
    asr_id, dec_id, rad_id, asr_fechaAsignacion, asr_fechaDevolucion,
    asr_estado, asr_observacionesAsignacion, asr_observacionesDevolucion, usa_id
)
VALUES
(1, 1, 2, '2026-04-01', NULL, 'ASIGNADO', 'Asignado para operaciones', NULL, 1);


----------######################################################
SELECT * FROM dbo.roles;
SELECT * FROM dbo.usuarios;
SELECT * FROM dbo.estadoradio;
SELECT * FROM dbo.modeloRadio;
SELECT * FROM dbo.clientes;
SELECT * FROM dbo.contratos;
SELECT * FROM dbo.detalleContratos;
SELECT * FROM dbo.radios;
SELECT * FROM dbo.proveedores;
SELECT * FROM dbo.asignacionRadios;


---###########################################################################################

-- Buscar contratos por estado
EXEC dbo.usp_Buscar_Contratos
    @cliente_id = 0,
    @estado = 'ACTIVO',
    @fecha_inicio = '',
    @fecha_fin = '';

-- Buscar contratos por cliente
EXEC dbo.usp_Buscar_Contratos
    @cliente_id = 1,
    @estado = '',
    @fecha_inicio = '',
    @fecha_fin = '';

-- Buscar contratos por fechas
EXEC dbo.usp_Buscar_Contratos
    @cliente_id = 0,
    @estado = '',
    @fecha_inicio = '2026-01-01',
    @fecha_fin = '2026-12-31';

-- Buscar modelo de radio
EXEC dbo.usp_Buscar_ModeloRadio
    @texto = 'Motorola';

EXEC dbo.usp_Buscar_ModeloRadio
    @texto = 'NX';

-- Buscar radios
EXEC dbo.usp_Buscar_Radio
    @p_texto = 'SERIE';

EXEC dbo.usp_Buscar_Radio
    @p_texto = 'DISPONIBLE';

EXEC dbo.usp_Buscar_Radio
    @p_texto = 'Kenwood';

-- Buscar usuario
EXEC dbo.usp_Buscar_Usuario
    @p_nombre = 'admin';

-- Listar estados
EXEC dbo.usp_radio_estados;

-- Listar modelos
EXEC dbo.usp_radio_modelos;

-- Obtener radio
EXEC dbo.usp_radio_obtener
    @p_rad_id = 1;

-- Insertar radio nuevo
EXEC dbo.usp_radio_guardar
    @p_rad_id = 0,
    @p_mod_id = 2,
    @p_esr_id = 1,
    @p_serie = 'SERIE-9999',
    @p_fecha_ingreso = '2026-04-13 10:00:00',
    @p_activo = 1;

-- Actualizar radio
EXEC dbo.usp_radio_guardar
    @p_rad_id = 1,
    @p_mod_id = 1,
    @p_esr_id = 3,
    @p_serie = 'SERIE-0001-EDIT',
    @p_fecha_ingreso = '2026-04-13 11:00:00',
    @p_activo = 1;

-- Verificar actualización
EXEC dbo.usp_radio_obtener
    @p_rad_id = 1;

-- Eliminar radio
EXEC dbo.usp_radio_eliminar
    @p_rad_id = 3;

-- Verificar eliminación
SELECT * FROM dbo.radios;

-----************************** CONTRATO ******************************

IF OBJECT_ID('dbo.usp_contrato_obtener', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_contrato_obtener;
GO

CREATE PROCEDURE dbo.usp_contrato_obtener
    @p_con_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.con_id,
        c.cli_id,
        c.con_numero,
        c.con_fechaInicio,
        c.con_fechaFin,
        c.con_estado,
        c.con_tipoContrato,
        c.con_valorTotal,
        c.con_valorMensual,
        c.con_observaciones,
        c.con_filaEliminada,
        c.usa_id,
        cl.cli_codigo,
        cl.cli_razonSocial
    FROM dbo.contratos c
    INNER JOIN dbo.clientes cl
        ON cl.cli_id = c.cli_id
    WHERE c.con_id = @p_con_id
      AND ISNULL(c.con_filaEliminada, 0) = 0;
END;
GO


IF OBJECT_ID('dbo.usp_contrato_guardar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_contrato_guardar;
GO

CREATE PROCEDURE dbo.usp_contrato_guardar
    @p_con_id            INT,
    @p_cli_id            INT,
    @p_con_numero        VARCHAR(100),
    @p_con_fechaInicio   DATE,
    @p_con_fechaFin      DATE,
    @p_con_estado        VARCHAR(50),
    @p_con_tipoContrato  VARCHAR(50),
    @p_con_valorTotal    DECIMAL(12,2),
    @p_con_valorMensual  DECIMAL(10,2),
    @p_con_observaciones VARCHAR(MAX),
    @p_usa_id            INT
AS
BEGIN
    SET NOCOUNT ON;

    IF ISNULL(@p_con_id, 0) = 0
    BEGIN
        INSERT INTO dbo.contratos
        (
            cli_id,
            con_numero,
            con_fechaInicio,
            con_fechaFin,
            con_estado,
            con_tipoContrato,
            con_valorTotal,
            con_valorMensual,
            con_observaciones,
            con_filaEliminada,
            usa_id
        )
        VALUES
        (
            @p_cli_id,
            @p_con_numero,
            @p_con_fechaInicio,
            @p_con_fechaFin,
            @p_con_estado,
            @p_con_tipoContrato,
            @p_con_valorTotal,
            @p_con_valorMensual,
            @p_con_observaciones,
            0,
            @p_usa_id
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT) AS con_id;
    END
    ELSE
    BEGIN
        UPDATE dbo.contratos
        SET
            cli_id            = @p_cli_id,
            con_numero        = @p_con_numero,
            con_fechaInicio   = @p_con_fechaInicio,
            con_fechaFin      = @p_con_fechaFin,
            con_estado        = @p_con_estado,
            con_tipoContrato  = @p_con_tipoContrato,
            con_valorTotal    = @p_con_valorTotal,
            con_valorMensual  = @p_con_valorMensual,
            con_observaciones = @p_con_observaciones,
            usa_id            = @p_usa_id
        WHERE con_id = @p_con_id;

        SELECT @p_con_id AS con_id;
    END
END;
GO


IF OBJECT_ID('dbo.usp_contrato_eliminar_logico', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_contrato_eliminar_logico;
GO

CREATE PROCEDURE dbo.usp_contrato_eliminar_logico
    @p_con_id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.contratos
    SET con_filaEliminada = 1
    WHERE con_id = @p_con_id
      AND ISNULL(con_filaEliminada, 0) = 0;

    SELECT @@ROWCOUNT AS filas_afectadas;
END;
GO



--**************************************
---ASIGNACION DE RADIOS ------------
--**************************************

IF OBJECT_ID('dbo.usp_asignacionRadio_listar_por_detalle', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_asignacionRadio_listar_por_detalle;
GO

CREATE PROCEDURE dbo.usp_asignacionRadio_listar_por_detalle
    @p_dec_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.asr_id,
        a.dec_id,
        a.rad_id,
        a.asr_fechaAsignacion,
        a.asr_fechaDevolucion,
        a.asr_estado,
        a.asr_observacionesAsignacion,
        a.asr_observacionesDevolucion,
        a.asr_filaEliminada,
        a.usa_id,
        r.serie,
        mr.mod_codigo,
        mr.mod_descripcion,
        e.esr_descripcion
    FROM dbo.asignacionRadios a
    INNER JOIN dbo.radios r
        ON r.rad_id = a.rad_id
    LEFT JOIN dbo.modeloRadio mr
        ON mr.mod_id = r.mod_id
    LEFT JOIN dbo.estadoradio e
        ON e.esr_id = r.esr_id
    WHERE a.dec_id = @p_dec_id
      AND ISNULL(a.asr_filaEliminada, 0) = 0
    ORDER BY a.asr_id;
END;
GO


IF OBJECT_ID('dbo.usp_asignacionRadio_obtener', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_asignacionRadio_obtener;
GO

CREATE PROCEDURE dbo.usp_asignacionRadio_obtener
    @p_asr_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.asr_id,
        a.dec_id,
        a.rad_id,
        a.asr_fechaAsignacion,
        a.asr_fechaDevolucion,
        a.asr_estado,
        a.asr_observacionesAsignacion,
        a.asr_observacionesDevolucion,
        a.asr_filaEliminada,
        a.usa_id,
        r.serie,
        mr.mod_codigo,
        mr.mod_descripcion,
        e.esr_descripcion
    FROM dbo.asignacionRadios a
    INNER JOIN dbo.radios r
        ON r.rad_id = a.rad_id
    LEFT JOIN dbo.modeloRadio mr
        ON mr.mod_id = r.mod_id
    LEFT JOIN dbo.estadoradio e
        ON e.esr_id = r.esr_id
    WHERE a.asr_id = @p_asr_id
      AND ISNULL(a.asr_filaEliminada, 0) = 0;
END;
GO


IF OBJECT_ID('dbo.usp_radio_disponibles_por_modelo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_radio_disponibles_por_modelo;
GO

CREATE PROCEDURE dbo.usp_radio_disponibles_por_modelo
    @p_mod_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.rad_id,
        r.mod_id,
        r.esr_id,
        r.serie,
        r.fecha_ingreso,
        r.rad_activo,
        mr.mod_codigo,
        mr.mod_descripcion,
        e.esr_descripcion
    FROM dbo.radios r
    LEFT JOIN dbo.modeloRadio mr
        ON mr.mod_id = r.mod_id
    LEFT JOIN dbo.estadoradio e
        ON e.esr_id = r.esr_id
    WHERE r.mod_id = @p_mod_id
      AND ISNULL(r.rad_filaeliminado, 0) = 0
      AND ISNULL(r.rad_activo, 0) = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.asignacionRadios a
          WHERE a.rad_id = r.rad_id
            AND ISNULL(a.asr_filaEliminada, 0) = 0
            AND (a.asr_estado = 'ASIGNADO' OR a.asr_fechaDevolucion IS NULL)
      )
    ORDER BY r.rad_id;
END;
GO


IF OBJECT_ID('dbo.usp_asignacionRadio_guardar', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_asignacionRadio_guardar;
GO

CREATE PROCEDURE dbo.usp_asignacionRadio_guardar
    @p_asr_id                       INT,
    @p_dec_id                       INT,
    @p_rad_id                       INT,
    @p_asr_fechaAsignacion          DATE,
    @p_asr_observacionesAsignacion  VARCHAR(MAX),
    @p_usa_id                       INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.asignacionRadios
        WHERE rad_id = @p_rad_id
          AND ISNULL(asr_filaEliminada, 0) = 0
          AND (asr_estado = 'ASIGNADO' OR asr_fechaDevolucion IS NULL)
          AND (@p_asr_id = 0 OR asr_id <> @p_asr_id)
    )
    BEGIN
        RAISERROR('El radio ya se encuentra asignado.', 16, 1);
        RETURN;
    END

    IF ISNULL(@p_asr_id, 0) = 0
    BEGIN
        DECLARE @nuevoId INT;
        SELECT @nuevoId = ISNULL(MAX(asr_id), 0) + 1
        FROM dbo.asignacionRadios;

        INSERT INTO dbo.asignacionRadios
        (
            asr_id,
            dec_id,
            rad_id,
            asr_fechaAsignacion,
            asr_fechaDevolucion,
            asr_estado,
            asr_observacionesAsignacion,
            asr_observacionesDevolucion,
            usa_id
        )
        VALUES
        (
            @nuevoId,
            @p_dec_id,
            @p_rad_id,
            @p_asr_fechaAsignacion,
            NULL,
            'ASIGNADO',
            @p_asr_observacionesAsignacion,
            NULL,
            @p_usa_id
        );

        SELECT @nuevoId AS asr_id;
    END
    ELSE
    BEGIN
        UPDATE dbo.asignacionRadios
        SET
            dec_id = @p_dec_id,
            rad_id = @p_rad_id,
            asr_fechaAsignacion = @p_asr_fechaAsignacion,
            asr_observacionesAsignacion = @p_asr_observacionesAsignacion,
            usa_id = @p_usa_id
        WHERE asr_id = @p_asr_id;

        SELECT @p_asr_id AS asr_id;
    END
END;
GO


IF OBJECT_ID('dbo.usp_asignacionRadio_devolver', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_asignacionRadio_devolver;
GO

CREATE PROCEDURE dbo.usp_asignacionRadio_devolver
    @p_asr_id                        INT,
    @p_asr_fechaDevolucion           DATE,
    @p_asr_observacionesDevolucion   VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.asignacionRadios
    SET
        asr_fechaDevolucion = @p_asr_fechaDevolucion,
        asr_estado = 'DEVUELTO',
        asr_observacionesDevolucion = @p_asr_observacionesDevolucion
    WHERE asr_id = @p_asr_id
      AND ISNULL(asr_filaEliminada, 0) = 0;

    SELECT @@ROWCOUNT AS filas_afectadas;
END;
GO


IF OBJECT_ID('dbo.usp_asignacionRadio_eliminar_logico', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_asignacionRadio_eliminar_logico;
GO

CREATE PROCEDURE dbo.usp_asignacionRadio_eliminar_logico
    @p_asr_id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.asignacionRadios
    SET asr_filaEliminada = 1
    WHERE asr_id = @p_asr_id
      AND ISNULL(asr_filaEliminada, 0) = 0;

    SELECT @@ROWCOUNT AS filas_afectadas;
END;
GO