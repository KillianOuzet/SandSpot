-- Suppression des tables existantes (dans l'ordre inverse des dépendances)
DROP TABLE IF EXISTS "join" CASCADE;
DROP TABLE IF EXISTS review CASCADE;
DROP TABLE IF EXISTS message CASCADE;
DROP TABLE IF EXISTS alert CASCADE;
DROP TABLE IF EXISTS level CASCADE;
DROP TABLE IF EXISTS zone CASCADE;
DROP TABLE IF EXISTS "user" CASCADE;
DROP TABLE IF EXISTS role CASCADE;

-- 1. Table ROLE
CREATE TABLE role (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- 2. Table USER
CREATE TABLE "user" (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role_id INT NOT NULL,
    CONSTRAINT fk_user_role FOREIGN KEY (role_id) REFERENCES role(id) ON DELETE RESTRICT
);

-- 3. Table ZONE (Coordonnées séparées + Code Postal)
CREATE TABLE zone (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    latitude DOUBLE PRECISION NOT NULL,
    longitude DOUBLE PRECISION NOT NULL,
    address VARCHAR(255) NOT NULL,
    city VARCHAR(100) NOT NULL,
    postal_code VARCHAR(10) NOT NULL
);

-- 4. Table LEVEL (Ex: Débutant, Intermédiaire, Avancé)
CREATE TABLE level (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- 5. Table ALERT (Nombre max de joueurs + Relation vers Level)
CREATE TABLE alert (
    id SERIAL PRIMARY KEY,
    description TEXT,
    date_time TIMESTAMP WITH TIME ZONE NOT NULL,
    max_players INT NOT NULL CHECK (max_players > 0),
    ball BOOLEAN DEFAULT FALSE,
    net BOOLEAN DEFAULT FALSE,
    zone_id INT NOT NULL,
    level_id INT NOT NULL,
    creator_id INT NOT NULL,
    CONSTRAINT fk_alert_zone FOREIGN KEY (zone_id) REFERENCES zone(id) ON DELETE CASCADE,
    CONSTRAINT fk_alert_level FOREIGN KEY (level_id) REFERENCES level(id) ON DELETE RESTRICT,
    CONSTRAINT fk_alert_creator FOREIGN KEY (creator_id) REFERENCES "user"(id) ON DELETE CASCADE
);

-- 6. Table JOIN (Anciennement LIKE - Un utilisateur rejoint une alerte)
CREATE TABLE "join" (
    user_id INT NOT NULL,
    alert_id INT NOT NULL,
    PRIMARY KEY (user_id, alert_id),
    CONSTRAINT fk_join_user FOREIGN KEY (user_id) REFERENCES "user"(id) ON DELETE CASCADE,
    CONSTRAINT fk_join_alert FOREIGN KEY (alert_id) REFERENCES alert(id) ON DELETE CASCADE
);

-- 7. Table MESSAGE
CREATE TABLE message (
    id SERIAL PRIMARY KEY,
    content TEXT NOT NULL,
    sent_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    sender_id INT NOT NULL,
    alert_id INT NOT NULL,
    CONSTRAINT fk_message_sender FOREIGN KEY (sender_id) REFERENCES "user"(id) ON DELETE CASCADE,
    CONSTRAINT fk_message_alert FOREIGN KEY (alert_id) REFERENCES alert(id) ON DELETE CASCADE
);

-- 8. Table REVIEW
CREATE TABLE review (
    id SERIAL PRIMARY KEY,
    rating INT NOT NULL CHECK (rating >= 1 AND rating <= 5),
    comment TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    author_id INT NOT NULL,
    target_id INT NOT NULL,
    CONSTRAINT fk_review_author FOREIGN KEY (author_id) REFERENCES "user"(id) ON DELETE CASCADE,
    CONSTRAINT fk_review_target FOREIGN KEY (target_id) REFERENCES alert(id) ON DELETE CASCADE
);

-- Données initiales pour la table LEVEL
INSERT INTO level (name) VALUES
     ('Débutant'),
     ('Intermédiaire'),
     ('Avancé'),
     ('Tous niveaux');

-- Données initiales pour la table ROLE
INSERT INTO role (name) VALUES
    ('Admin'),
    ('Joueur');