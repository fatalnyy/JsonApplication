CREATE DATABASE topkey;
USE topkey;

CREATE TABLE GroupsTable(
	id     int          NOT NULL,
	name   varchar(255) NOT NULL,
	active bit          NOT NULL,
	PRIMARY KEY (id)
);

CREATE TABLE GroupModuleAccess(
	groupId  int NOT NULL,
	moduleId int NOT NULL,
	PRIMARY KEY (groupId, moduleId)
);

CREATE TABLE Users(
	id       int 	      NOT NULL,
	name     varchar(255) NOT NULL,
	surname  varchar(255) NOT NULL,
	groupId  int 		  NOT NULL,
	male     bit 		  NOT NULL,
	login    varchar(255) NOT NULL,
	salt     varchar(255) NOT NULL,
	password varchar(255) NOT NULL,
	active   bit 		  NOT NULL,
	PRIMARY KEY(id)
);

CREATE TABLE SystemModules(
	id   int 		  NOT NULL,
	name varchar(255) NOT NULL,
	PRIMARY KEY(id)
);

CREATE VIEW a
AS  
SELECT user_.groupId, group_.name AS groupName, group_.active AS groupActive,
	   user_.id AS userId, user_.name AS userName, user_.surname, user_.male, user_.login, user_.salt, user_.password, user_.active AS userActive,
	   sm.name as SystemModulesName
FROM   GroupsTable group_
JOIN   Users  user_
	ON group_.id = user_.groupId
JOIN   GroupModuleAccess gma
	ON group_.id = gma.groupId 
JOIN   SystemModules sm
	ON gma.moduleId = sm.id
ORDER BY group_.name, user_.name DESC;

CREATE VIEW b
AS
SELECT group_.id, group_.name, count(user_.id) AS number_people
FROM   GroupsTable group_
JOIN   Users  user_
	ON group_.id = user_.groupId
GROUP BY 1,2;

SELECT a.groupName, b.number_people
FROM   a
JOIN   b 
	ON b.id = a.groupId
WHERE  b.id > 1
ORDER BY 1,2;
