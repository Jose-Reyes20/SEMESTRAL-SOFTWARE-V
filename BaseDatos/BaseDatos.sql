-- 1. CREACIÓN DE LA BASE DE DATOS
CREATE DATABASE IF NOT EXISTS ecommerce_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- 2. SELECCIÓN DE LA BASE DE DATOS
USE ecommerce_db;

-- Establecer el delimitador para permitir la creación de TRIGGERS
DELIMITER $$

-- -----------------------------------------------------
-- 3. Definición de Tablas (Cláusula VISIBLE ELIMINADA)
-- -----------------------------------------------------

-- Tabla: cliente
CREATE TABLE IF NOT EXISTS cliente (
  id INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(255) NOT NULL,
  apellido VARCHAR(255) NOT NULL,
  direccion VARCHAR(255) NOT NULL,
  telefono VARCHAR(255) NOT NULL,
  correo VARCHAR(255) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  UNIQUE INDEX correo_unique (correo ASC)
);

-- Trigger para actualizar el timestamp en la tabla cliente
CREATE TRIGGER trg_cliente_update_timestamp
BEFORE UPDATE ON cliente
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: usuario
CREATE TABLE IF NOT EXISTS usuario (
  id INT NOT NULL AUTO_INCREMENT,
  usuario VARCHAR(255) NOT NULL,
  contrasena VARCHAR(255) NOT NULL,
  rol VARCHAR(255) NOT NULL COMMENT 'Puede ser admin | cliente',
  cliente_id INT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  update_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  UNIQUE INDEX usuario_unique (usuario ASC),
  INDEX usuario_cliente_id_fk_idx (cliente_id ASC),
  CONSTRAINT usuario_cliente_id_fk
    FOREIGN KEY (cliente_id)
    REFERENCES cliente (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT chk_rol CHECK (rol IN ('admin', 'cliente'))
);

-- Trigger para actualizar el timestamp en la tabla usuario
CREATE TRIGGER trg_usuario_update_timestamp
BEFORE UPDATE ON usuario
FOR EACH ROW
BEGIN
    SET NEW.update_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: articulo
CREATE TABLE IF NOT EXISTS articulo (
  id INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(255) NOT NULL,
  descripcion VARCHAR(255) NULL,
  precio DECIMAL(10, 2) NOT NULL,
  stock INT NOT NULL,
  paga_itbms BOOLEAN NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
);

-- Trigger para actualizar el timestamp en la tabla articulo
CREATE TRIGGER trg_articulo_update_timestamp
BEFORE UPDATE ON articulo
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: categoria
CREATE TABLE IF NOT EXISTS categoria (
  id INT NOT NULL AUTO_INCREMENT,
  nombre VARCHAR(255) NOT NULL,
  categoria_padre_id INT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  INDEX categoria_padre_id_fk_idx (categoria_padre_id ASC),
  CONSTRAINT categoria_padre_id_fk
    FOREIGN KEY (categoria_padre_id)
    REFERENCES categoria (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);

-- Trigger para actualizar el timestamp en la tabla categoria
CREATE TRIGGER trg_categoria_update_timestamp
BEFORE UPDATE ON categoria
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: articulo_categoria (Tabla de relación N:M)
CREATE TABLE IF NOT EXISTS articulo_categoria (
  id INT NOT NULL AUTO_INCREMENT,
  id_articulo INT NOT NULL,
  id_categoria INT NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  INDEX fk_articulo_idx (id_articulo ASC),
  INDEX fk_categoria_idx (id_categoria ASC),
  CONSTRAINT fk_articulo
    FOREIGN KEY (id_articulo)
    REFERENCES articulo (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT fk_categoria
    FOREIGN KEY (id_categoria)
    REFERENCES categoria (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

-- Trigger para actualizar el timestamp en la tabla articulo_categoria
CREATE TRIGGER trg_articulo_categoria_update_timestamp
BEFORE UPDATE ON articulo_categoria
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: foto
CREATE TABLE IF NOT EXISTS foto (
  id INT NOT NULL AUTO_INCREMENT,
  articulo_id INT NOT NULL,
  foto_principal TEXT NOT NULL,
  foto_2 TEXT NULL,
  foto_3 TEXT NULL,
  foto_4 TEXT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  INDEX fk_articulo_foto_idx (articulo_id ASC),
  CONSTRAINT fk_articulo_foto
    FOREIGN KEY (articulo_id)
    REFERENCES articulo (id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
);

-- Trigger para actualizar el timestamp en la tabla foto
CREATE TRIGGER trg_articulo_foto_updated_at
BEFORE UPDATE ON foto
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: cupon
CREATE TABLE IF NOT EXISTS cupon (
  id INT NOT NULL AUTO_INCREMENT,
  codigo VARCHAR(255) NOT NULL,
  descuento DECIMAL(10, 2) NOT NULL,
  estado BOOLEAN NOT NULL DEFAULT TRUE,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  UNIQUE INDEX codigo_unico_idx (codigo ASC)
);

-- Trigger para actualizar el timestamp en la tabla cupon
CREATE TRIGGER trg_cupon_update_timestamp
BEFORE UPDATE ON cupon
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: factura
CREATE TABLE IF NOT EXISTS factura (
  id INT NOT NULL AUTO_INCREMENT,
  cupon_id INT NULL,
  subtotal DECIMAL(10, 2) NOT NULL,
  descuento DECIMAL(10, 2) DEFAULT 0,
  total DECIMAL(10, 2) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  fecha DATETIME NOT NULL,
  itbms DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
  usuario_id INT NOT NULL,
  PRIMARY KEY (id),
  INDEX factura_cupon_id_fkey_idx (cupon_id ASC),
  INDEX factura_usuario_id_fk_idx (usuario_id ASC),
  CONSTRAINT factura_cupon_id_fkey
    FOREIGN KEY (cupon_id)
    REFERENCES cupon (id)
    ON DELETE SET NULL
    ON UPDATE NO ACTION,
  CONSTRAINT factura_usuario_id_fk
    FOREIGN KEY (usuario_id)
    REFERENCES usuario (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT factura_descuento_check CHECK (descuento >= 0),
  CONSTRAINT factura_subtotal_check CHECK (subtotal >= 0),
  CONSTRAINT factura_total_check CHECK (total >= 0)
);

-- Trigger para actualizar el timestamp en la tabla factura
CREATE TRIGGER trg_factura_update_timestamp
BEFORE UPDATE ON factura
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: factura_detalle
CREATE TABLE IF NOT EXISTS factura_detalle (
  id INT NOT NULL AUTO_INCREMENT,
  articulo_id INT NOT NULL,
  precio_final DECIMAL(10, 2) NOT NULL,
  factura_id INT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  precio_unitario DECIMAL(10, 2) NOT NULL,
  PRIMARY KEY (id),
  INDEX factura_detalle_articulo_id_fkey_idx (articulo_id ASC),
  INDEX factura_detalle_factura_id_fkey_idx (factura_id ASC),
  CONSTRAINT factura_detalle_articulo_id_fkey
    FOREIGN KEY (articulo_id)
    REFERENCES articulo (id)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT factura_detalle_factura_id_fkey
    FOREIGN KEY (factura_id)
    REFERENCES factura (id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT factura_detalle_precio_final_check CHECK (precio_final >= 0)
);

-- Trigger para actualizar el timestamp en la tabla factura_detalle
CREATE TRIGGER trg_factura_detalle_update_timestamp
BEFORE UPDATE ON factura_detalle
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: orden
CREATE TABLE IF NOT EXISTS orden (
  id INT NOT NULL AUTO_INCREMENT,
  estado VARCHAR(255) NOT NULL,
  fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
  usuario_id INT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  cupon_id INT NULL,
  subtotal DECIMAL(10, 2) NOT NULL,
  total DECIMAL(10, 2) NOT NULL,
  descuento DECIMAL(10, 2) DEFAULT 0,
  itbms DECIMAL(10, 2) NOT NULL DEFAULT 0,
  PRIMARY KEY (id),
  INDEX fk_orden_usuario_idx (usuario_id ASC),
  INDEX orden_cupon_id_fk_idx (cupon_id ASC),
  CONSTRAINT fk_orden_usuario
    FOREIGN KEY (usuario_id)
    REFERENCES usuario (id)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT orden_cupon_id_fk
    FOREIGN KEY (cupon_id)
    REFERENCES cupon (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT orden_estado_check CHECK (estado IN ('pendiente', 'procesando', 'completada', 'cancelada'))
);

-- Trigger para actualizar el timestamp en la tabla orden
CREATE TRIGGER trg_orden_update_timestamp
BEFORE UPDATE ON orden
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- Tabla: orden_detalle
CREATE TABLE IF NOT EXISTS orden_detalle (
  id INT NOT NULL AUTO_INCREMENT,
  orden_id INT NOT NULL,
  articulo_id INT NOT NULL,
  cantidad INT NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  precio_unitario DECIMAL(10, 2) NOT NULL,
  precio_final DECIMAL(10, 2) NOT NULL,
  PRIMARY KEY (id),
  INDEX fk_orden_detalle_orden_idx (orden_id ASC),
  INDEX fk_orden_detalle_articulo_idx (articulo_id ASC),
  CONSTRAINT fk_orden_detalle_orden
    FOREIGN KEY (orden_id)
    REFERENCES orden (id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT fk_orden_detalle_articulo
    FOREIGN KEY (articulo_id)
    REFERENCES articulo (id)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT orden_detalle_cantidad_check CHECK (cantidad > 0)
);

-- Trigger para actualizar el timestamp en la tabla orden_detalle
CREATE TRIGGER trg_orden_detalle_update_timestamp
BEFORE UPDATE ON orden_detalle
FOR EACH ROW
BEGIN
    SET NEW.updated_at = CURRENT_TIMESTAMP();
END$$

-- 4. RESTAURAR DELIMITADOR
DELIMITER ;