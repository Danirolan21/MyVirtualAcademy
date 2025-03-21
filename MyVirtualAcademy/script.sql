
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[ID_Usuario] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Apellidos] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Pass_Hash] [varbinary](max) NOT NULL,
	[Salt] [nvarchar](50) NOT NULL,
	[Pass] [nvarchar](255) NOT NULL,
	[Fecha_Registro] [datetime] NULL,
	[Activo] [bit] NULL,
	[Ultimo_Acceso] [datetime] NULL,
	[Foto_Perfil] [nvarchar](255) NULL,
	[Telefono] [nvarchar](20) NULL,
	[Token_Recuperacion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID_Rol] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios_Roles]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios_Roles](
	[ID_Usuario] [int] NOT NULL,
	[ID_Rol] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Usuario] ASC,
	[ID_Rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vista_Usuarios_Con_Roles]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Vista_Usuarios_Con_Roles] AS
SELECT U.ID_Usuario, U.Nombre, U.Apellidos, U.Email, R.Nombre AS Rol, U.Activo
FROM Usuarios U
INNER JOIN Usuarios_Roles UR ON U.ID_Usuario = UR.ID_Usuario
INNER JOIN Roles R ON UR.ID_Rol = R.ID_Rol;
GO
/****** Object:  Table [dbo].[Cursos]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cursos](
	[ID_Curso] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[ID_Profesor] [int] NOT NULL,
	[ID_Profesor_Suplente] [int] NULL,
	[Fecha_Inicio] [date] NOT NULL,
	[Fecha_Fin] [date] NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[Imagen_Portada] [nvarchar](255) NULL,
 CONSTRAINT [PK__Cursos__DC72196F565F18AB] PRIMARY KEY CLUSTERED 
(
	[ID_Curso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inscripciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inscripciones](
	[ID_Inscripcion] [int] NOT NULL,
	[ID_Estudiante] [int] NOT NULL,
	[ID_Curso] [int] NOT NULL,
	[Fecha_Inscripcion] [datetime] NULL,
	[Estado] [varchar](20) NULL,
	[Rol_Inscrito] [varchar](20) NOT NULL,
	[Porcentaje_Completado] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Inscripcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vista_Inscripciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- 📌 Vista para ver Todas las Inscripciones
CREATE VIEW [dbo].[Vista_Inscripciones] AS
SELECT I.ID_Inscripcion, U.Nombre + ' ' + U.Apellidos AS Estudiante, C.Nombre AS Curso, I.Estado, I.Porcentaje_Completado
FROM Inscripciones I
INNER JOIN Usuarios U ON I.ID_Estudiante = U.ID_Usuario
INNER JOIN Cursos C ON I.ID_Curso = C.ID_Curso;
GO
/****** Object:  Table [dbo].[Asignaturas]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asignaturas](
	[ID_Asignatura] [int] NOT NULL,
	[ID_Curso] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Asignatura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vista_Asignaturas_Usuario]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Vista_Asignaturas_Usuario] AS
SELECT 
    U.ID_Usuario, 
    U.Nombre AS NombreUsuario, 
    C.ID_Curso,
    C.Nombre AS NombreCurso, 
    A.ID_Asignatura, 
    A.Nombre AS NombreAsignatura
FROM Inscripciones I
INNER JOIN Usuarios U ON I.ID_Estudiante = U.ID_Usuario
INNER JOIN Cursos C ON I.ID_Curso = C.ID_Curso
INNER JOIN Asignaturas A ON C.ID_Curso = A.ID_Curso
WHERE I.Estado = 'Activo'; -- Solo traer asignaturas de cursos activos

GO
/****** Object:  View [dbo].[Vista_Cursos_Detalles]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Vista_Cursos_Detalles] AS
SELECT 
    C.ID_Curso,
    C.Nombre AS Nombre_Curso,
    C.Descripcion,
    C.ID_Profesor,
    C.ID_Profesor_Suplente AS ID_Suplente,
    C.Fecha_Inicio,
    C.Fecha_Fin,
    C.Estado,
    C.Imagen_Portada,
    U.Nombre + ' ' + U.Apellidos AS Nombre_Profesor,
    (SELECT US.Nombre + ' ' + US.Apellidos FROM Usuarios US WHERE US.ID_Usuario = C.ID_Profesor_Suplente) AS Nombre_Suplente,
    (SELECT COUNT(*) FROM Asignaturas A WHERE A.ID_Curso = C.ID_Curso) AS Numero_Asignaturas,
    (SELECT COUNT(*) FROM Inscripciones I WHERE I.ID_Curso = C.ID_Curso) AS Numero_Alumnos
FROM Cursos C
INNER JOIN Usuarios U ON C.ID_Profesor = U.ID_Usuario;
GO
/****** Object:  Table [dbo].[Progreso_Inscripciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Progreso_Inscripciones](
	[ID_Inscripcion] [int] NOT NULL,
	[ID_Contenido] [int] NOT NULL,
	[Completo] [bit] NULL,
	[Fecha_Completado] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Inscripcion] ASC,
	[ID_Contenido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profesores_Asignaturas]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profesores_Asignaturas](
	[ID_Asignatura] [int] NOT NULL,
	[ID_Profesor] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Asignatura] ASC,
	[ID_Profesor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Temas]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temas](
	[ID_Tema] [int] NOT NULL,
	[ID_Asignatura] [int] NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[Orden] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Tema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contenidos]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contenidos](
	[ID_Contenido] [int] NOT NULL,
	[ID_Tema] [int] NOT NULL,
	[Titulo] [nvarchar](200) NOT NULL,
	[Tipo] [varchar](20) NOT NULL,
	[URL_Contenido] [nvarchar](255) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Orden] [int] NOT NULL,
	[Fecha_Publicacion] [datetime] NULL,
	[Fecha_Entrega] [datetime] NULL,
	[Puntuacion_Maxima] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Contenido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vista_Detalles_Asignatura]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Vista_Detalles_Asignatura] AS
SELECT 
    -- 📌 Información de la asignatura
    a.ID_Asignatura,
    a.Nombre AS Nombre_Asignatura,

    -- 📌 Profesores que la imparten
    p.ID_Usuario AS ID_Profesor,
    p.Nombre AS Nombre_Profesor,
    p.Apellidos AS Apellidos_Profesor,
    p.Foto_Perfil AS Foto_Perfil,

    -- 📌 Temas de la asignatura
    t.ID_Tema,
    t.Nombre AS Nombre_Tema,
    t.Orden AS Orden_Tema,

    -- 📌 Contenidos de cada tema
    c.ID_Contenido,
    c.Titulo AS Titulo_Contenido,
    c.Descripcion AS Descripcion_Contenido,
    c.Tipo AS Tipo_Contenido,
    c.Orden AS Orden_Contenido,
    c.Fecha_Publicacion,

    -- 📌 Progreso del estudiante en los contenidos (puede ser NULL si el estudiante no ha marcado como completado)
    pi.Completo AS Contenido_Completado

FROM Asignaturas a
    -- 🔗 Profesores que imparten la asignatura
    JOIN Profesores_Asignaturas pa ON a.ID_Asignatura = pa.ID_Asignatura
    JOIN Usuarios p ON pa.ID_Profesor = p.ID_Usuario

    -- 🔗 Temas dentro de la asignatura
    LEFT JOIN Temas t ON a.ID_Asignatura = t.ID_Asignatura

    -- 🔗 Contenidos dentro de los temas
    LEFT JOIN Contenidos c ON t.ID_Tema = c.ID_Tema

    -- 🔗 Progreso del estudiante en los contenidos (Se filtra más adelante en la consulta final)
    LEFT JOIN Progreso_Inscripciones pi ON c.ID_Contenido = pi.ID_Contenido;
GO
/****** Object:  Table [dbo].[Comentarios_Calificaciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comentarios_Calificaciones](
	[ID_Comentario] [int] NOT NULL,
	[ID_Calificacion] [int] NOT NULL,
	[ID_Autor] [int] NOT NULL,
	[Comentario] [nvarchar](max) NOT NULL,
	[Fecha_Comentario] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Comentario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Correcciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Correcciones](
	[ID_Correccion] [int] NOT NULL,
	[ID_Respuesta_Desarrollo] [int] NOT NULL,
	[ID_Profesor] [int] NOT NULL,
	[Calificacion] [decimal](5, 2) NULL,
	[Comentarios] [nvarchar](max) NULL,
	[Fecha_Correccion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Correccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entregas_Tareas]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entregas_Tareas](
	[ID_Entrega] [int] NOT NULL,
	[ID_Contenido] [int] NOT NULL,
	[ID_Estudiante] [int] NULL,
	[URL_Entrega] [nvarchar](255) NULL,
	[Fecha_Entrega] [datetime] NULL,
	[Estado] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Entrega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examenes]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examenes](
	[ID_Contenido] [int] NOT NULL,
	[Duracion_Minutos] [int] NULL,
	[Fecha_Publicacion_Notas] [datetime] NULL,
	[Intentos_Maximos] [int] NULL,
	[Penalizacion_Incorrecta] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Contenido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examenes_Usuarios]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examenes_Usuarios](
	[ID_Intento] [int] NOT NULL,
	[ID_Contenido] [int] NOT NULL,
	[ID_Estudiante] [int] NOT NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Tiempo_Excedido] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Intento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grupos_Chat]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grupos_Chat](
	[ID_Grupo] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Es_Privado] [bit] NULL,
	[Fecha_Creacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Grupo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Historial_Calificaciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Historial_Calificaciones](
	[ID_Calificacion] [int] NOT NULL,
	[ID_Estudiante] [int] NOT NULL,
	[ID_Contenido] [int] NOT NULL,
	[Calificacion] [decimal](5, 2) NOT NULL,
	[Fecha_Calificacion] [datetime] NULL,
	[ID_Profesor_Calificador] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Calificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mensajes_Chat]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mensajes_Chat](
	[ID_Mensaje] [int] NOT NULL,
	[ID_Grupo] [int] NOT NULL,
	[ID_Emisor] [int] NOT NULL,
	[Contenido] [nvarchar](max) NOT NULL,
	[Fecha_Envio] [datetime] NULL,
	[Leido] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Mensaje] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Miembros_Grupo]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Miembros_Grupo](
	[ID_Grupo] [int] NOT NULL,
	[ID_Usuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Grupo] ASC,
	[ID_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificaciones]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificaciones](
	[ID_Notificacion] [int] NOT NULL,
	[ID_Usuario] [int] NOT NULL,
	[Tipo] [varchar](20) NOT NULL,
	[Mensaje] [nvarchar](max) NOT NULL,
	[Estado] [varchar](20) NULL,
	[Fecha_Envio] [datetime] NULL,
	[Enviado_Por] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Notificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Preguntas]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Preguntas](
	[ID_Pregunta] [int] NOT NULL,
	[ID_Contenido] [int] NOT NULL,
	[Tipo] [varchar](20) NULL,
	[Valor_Pregunta] [decimal](5, 2) NOT NULL,
	[Enunciado] [nvarchar](max) NOT NULL,
	[Es_Multiple] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Pregunta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Respuestas]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Respuestas](
	[ID_Respuesta] [int] NOT NULL,
	[ID_Pregunta] [int] NOT NULL,
	[Texto] [nvarchar](255) NOT NULL,
	[Es_Correcta] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Respuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Respuestas_Desarrollo]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Respuestas_Desarrollo](
	[ID_Respuesta_Desarrollo] [int] NOT NULL,
	[ID_Intento] [int] NOT NULL,
	[ID_Pregunta] [int] NOT NULL,
	[ID_Estudiante] [int] NOT NULL,
	[Respuesta] [nvarchar](max) NOT NULL,
	[Estado] [varchar](20) NULL,
	[Fecha_Entrega] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Respuesta_Desarrollo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Respuestas_Usuarios]    Script Date: 14/03/2025 8:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Respuestas_Usuarios](
	[ID_Respuesta_Usuario] [int] NOT NULL,
	[ID_Intento] [int] NOT NULL,
	[ID_Estudiante] [int] NOT NULL,
	[ID_Pregunta] [int] NOT NULL,
	[ID_Respuesta] [int] NOT NULL,
	[Fecha_Respuesta] [datetime] NULL,
	[Es_Correcta] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_Respuesta_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Asignaturas] ([ID_Asignatura], [ID_Curso], [Nombre]) VALUES (1, 1, N'Desarrollo Front con JavaScript, Jquery, React, Angular y Vuew.JS')
INSERT [dbo].[Asignaturas] ([ID_Asignatura], [ID_Curso], [Nombre]) VALUES (2, 1, N'Microsoft Power Platform')
INSERT [dbo].[Asignaturas] ([ID_Asignatura], [ID_Curso], [Nombre]) VALUES (3, 1, N'Programando en C#')
INSERT [dbo].[Asignaturas] ([ID_Asignatura], [ID_Curso], [Nombre]) VALUES (4, 1, N'Desarrollo de aplicaciones Web ASP.NET Core')
INSERT [dbo].[Asignaturas] ([ID_Asignatura], [ID_Curso], [Nombre]) VALUES (5, 1, N'Orientacion Laboral')
GO
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (1, 1, N'Aprender a usar Jquery', N'Documento', N'apuntesJquery.pdf', N'En este .pdf podéis ver los principales usos de Jquery, como usarlo e importarlo a vuestros proyectos', 1, CAST(N'2025-03-12T20:23:54.060' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (2, 1, N'Video Youtube', N'Video', N'videoJquery.mp4', N'Metete a este video y contempla de lo que es capaz Jquery', 2, CAST(N'2025-03-13T10:41:07.880' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (3, 1, N'Examen Jquery', N'Examen', N'examenJquery.word', N'Adjunto el documento .word con todas las expecificaciones del examen, teneis 2 horas para realizarlo', 1, CAST(N'2025-03-13T10:42:17.410' AS DateTime), CAST(N'2025-03-14T10:45:00.000' AS DateTime), CAST(10.00 AS Decimal(5, 2)))
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (4, 1, N'Ejercicio Zapatillas Jquery', N'Tarea', N'tareaJquery.pdf', N'Debéis recrear esta web de zapatillas con Jquery, dentro del .pdf esta explicado cada punto, se entregará un .html', 2, CAST(N'2025-03-13T10:53:36.513' AS DateTime), CAST(N'2025-03-13T13:00:00.000' AS DateTime), CAST(10.00 AS Decimal(5, 2)))
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (5, 2, N'Apuntes React', N'Documento', N'react.pdf', N'Apuntes react', 1, CAST(N'2025-03-13T13:34:32.600' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (6, 3, N'Apuntes Angular', N'Documento', N'951a4100-4ccd-4ad7-a555-528929c1b9f1_Apuntes_Angular_Prueba.pdf', N'Este documento recopila los conceptos fundamentales de Angular, incluyendo su arquitectura basada en componentes, el uso de módulos, directivas, servicios e inyección de dependencias. También se abordan buenas prácticas y ejemplos prácticos para el desarrollo de aplicaciones web dinámicas y escalables con este framework.', 1, CAST(N'2025-03-13T20:23:17.693' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (7, 3, N'Video Angular', N'Video', N'bcd4f6a0-7657-4253-8f3f-f76e981041c8_Video_Angular_Prueba.mp4', N'Este video ofrece una introducción al framework Angular, explicando sus principales características y casos de uso. Aprenderás sobre la estructura de una aplicación Angular, el sistema de módulos, componentes y cómo empezar con tu primer proyecto. Ideal para quienes se inician en el desarrollo con Angular.', 2, CAST(N'2025-03-13T20:34:57.663' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (8, 2, N'Maravilloso Video', N'Video', N'e52c9c00-94c2-4cd0-8962-d15d7368ef05_yt1z.net - Nyan Cat Official (480p).mp4', N'Este video ofrece una introducción al framework React, explicando sus principales características y casos de uso. Aprenderás sobre la estructura de una aplicación React, el sistema de módulos, componentes y cómo empezar con tu primer proyecto. Ideal para quienes se inician en el desarrollo con React.', 2, CAST(N'2025-03-13T20:43:02.523' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (9, 3, N'Visita la pagina Oficial de Angular', N'Enlace', N'https://angular.dev/', N'Accede al sitio web oficial de Angular para obtener documentación actualizada, guías de desarrollo, tutoriales y noticias sobre nuevas versiones del framework.', 3, CAST(N'2025-03-13T20:46:38.353' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (10, 2, N'Enlace', N'Enlace', N'https://es.react.dev/', N'enlace', 1, CAST(N'2025-03-13T20:47:39.253' AS DateTime), NULL, NULL)
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (11, 3, N'Examen Angular', N'Examen', N'3e1e539e-0b73-482d-b4c9-2d0c1b48e880_Instrucciones_Examen_Angular.docx', N'Este examen evalúa los conocimientos fundamentales sobre Angular, incluyendo su arquitectura, componentes, directivas y servicios. Los estudiantes deberán responder preguntas teóricas y realizar ejercicios prácticos para demostrar su comprensión del framework.', 4, CAST(N'2025-03-13T20:51:04.117' AS DateTime), CAST(N'2025-03-13T22:30:00.000' AS DateTime), CAST(10.00 AS Decimal(5, 2)))
INSERT [dbo].[Contenidos] ([ID_Contenido], [ID_Tema], [Titulo], [Tipo], [URL_Contenido], [Descripcion], [Orden], [Fecha_Publicacion], [Fecha_Entrega], [Puntuacion_Maxima]) VALUES (12, 2, N'Tarea React', N'Tarea', N'25e7f88e-4cda-458c-a5cf-358d4fd90438_Instrucciones_EjercicioReact.pdf', N'En esta tarea, los estudiantes deberán desarrollar una aplicación simple utilizando React. El objetivo es poner en práctica conceptos fundamentales como el manejo de componentes, el estado y los eventos en React. La tarea debe incluir un formulario interactivo que permita al usuario ingresar datos y mostrarlos en una lista. Además, se evaluará el uso adecuado de hooks como useState y useEffect.', 2, CAST(N'2025-03-13T23:57:04.717' AS DateTime), CAST(N'2025-03-14T00:50:00.000' AS DateTime), CAST(10.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[Cursos] ([ID_Curso], [Nombre], [Descripcion], [ID_Profesor], [ID_Profesor_Suplente], [Fecha_Inicio], [Fecha_Fin], [Estado], [Imagen_Portada]) VALUES (1, N'Master Desarrollo Web FullStack + MultiCloud', N'Este máster está diseñado para jóvenes con conocimientos informáticos en programación que desean especializarse en tecnologías de desarrollo Front y Back, así como en soluciones cloud.', 2, NULL, CAST(N'2024-09-29' AS Date), CAST(N'2025-06-04' AS Date), N'Activo', N'api-512d36c09662682717108a38bbb5c57d.gif')
INSERT [dbo].[Cursos] ([ID_Curso], [Nombre], [Descripcion], [ID_Profesor], [ID_Profesor_Suplente], [Fecha_Inicio], [Fecha_Fin], [Estado], [Imagen_Portada]) VALUES (2, N'Prueba', N'prueba', 4, NULL, CAST(N'2024-09-13' AS Date), CAST(N'2025-05-31' AS Date), N'Borrador', N'5433707.png')
GO
INSERT [dbo].[Entregas_Tareas] ([ID_Entrega], [ID_Contenido], [ID_Estudiante], [URL_Entrega], [Fecha_Entrega], [Estado]) VALUES (1, 12, 3, N'/uploads/tareas/12/3_20250314041126_Instrucciones_EjercicioReact.pdf', CAST(N'2025-03-14T04:11:26.643' AS DateTime), N'Pendiente')
GO
INSERT [dbo].[Examenes] ([ID_Contenido], [Duracion_Minutos], [Fecha_Publicacion_Notas], [Intentos_Maximos], [Penalizacion_Incorrecta]) VALUES (11, 5, CAST(N'2025-03-13T22:50:00.000' AS DateTime), 2, CAST(5.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[Inscripciones] ([ID_Inscripcion], [ID_Estudiante], [ID_Curso], [Fecha_Inscripcion], [Estado], [Rol_Inscrito], [Porcentaje_Completado]) VALUES (1, 3, 1, CAST(N'2025-03-12T21:12:20.337' AS DateTime), N'Activo', N'Estudiante', CAST(41.67 AS Decimal(5, 2)))
GO
INSERT [dbo].[Profesores_Asignaturas] ([ID_Asignatura], [ID_Profesor]) VALUES (1, 2)
INSERT [dbo].[Profesores_Asignaturas] ([ID_Asignatura], [ID_Profesor]) VALUES (2, 4)
GO
INSERT [dbo].[Progreso_Inscripciones] ([ID_Inscripcion], [ID_Contenido], [Completo], [Fecha_Completado]) VALUES (1, 1, 1, CAST(N'2025-03-14T04:06:34.413' AS DateTime))
INSERT [dbo].[Progreso_Inscripciones] ([ID_Inscripcion], [ID_Contenido], [Completo], [Fecha_Completado]) VALUES (1, 2, 1, CAST(N'2025-03-14T04:06:40.843' AS DateTime))
INSERT [dbo].[Progreso_Inscripciones] ([ID_Inscripcion], [ID_Contenido], [Completo], [Fecha_Completado]) VALUES (1, 5, 1, CAST(N'2025-03-14T04:07:00.770' AS DateTime))
INSERT [dbo].[Progreso_Inscripciones] ([ID_Inscripcion], [ID_Contenido], [Completo], [Fecha_Completado]) VALUES (1, 8, 1, CAST(N'2025-03-14T04:07:07.977' AS DateTime))
INSERT [dbo].[Progreso_Inscripciones] ([ID_Inscripcion], [ID_Contenido], [Completo], [Fecha_Completado]) VALUES (1, 10, 1, CAST(N'2025-03-14T04:07:04.877' AS DateTime))
INSERT [dbo].[Progreso_Inscripciones] ([ID_Inscripcion], [ID_Contenido], [Completo], [Fecha_Completado]) VALUES (1, 12, 1, CAST(N'2025-03-14T04:11:26.757' AS DateTime))
GO
INSERT [dbo].[Roles] ([ID_Rol], [Nombre]) VALUES (3, N'Administrador')
INSERT [dbo].[Roles] ([ID_Rol], [Nombre]) VALUES (2, N'Estudiante')
INSERT [dbo].[Roles] ([ID_Rol], [Nombre]) VALUES (1, N'Profesor')
INSERT [dbo].[Roles] ([ID_Rol], [Nombre]) VALUES (4, N'Tutor')
GO
INSERT [dbo].[Temas] ([ID_Tema], [ID_Asignatura], [Nombre], [Orden]) VALUES (1, 1, N'Modulo 1: Jquery', 1)
INSERT [dbo].[Temas] ([ID_Tema], [ID_Asignatura], [Nombre], [Orden]) VALUES (2, 1, N'Modulo 2: React', 1)
INSERT [dbo].[Temas] ([ID_Tema], [ID_Asignatura], [Nombre], [Orden]) VALUES (3, 1, N'Modulo 3: Angular', 1)
INSERT [dbo].[Temas] ([ID_Tema], [ID_Asignatura], [Nombre], [Orden]) VALUES (4, 1, N'Modulo 4: Vue.Js', 4)
GO
INSERT [dbo].[Usuarios] ([ID_Usuario], [Nombre], [Apellidos], [Email], [Pass_Hash], [Salt], [Pass], [Fecha_Registro], [Activo], [Ultimo_Acceso], [Foto_Perfil], [Telefono], [Token_Recuperacion]) VALUES (1, N'Admin', N'Suprem', N'admin.supremo@tajamar365.com', 0x844935836AE1C4B9C7DF2229678F58B0170A830238EDEF5B9C7F6F123755AFDA, N'ê¾ÝÏ¦äË·ôàbßÐ5ÙÃòßcp²6·³6Æ}fî#OZ=÷Jb­ü', N'admin', CAST(N'2025-03-12T19:25:17.123' AS DateTime), 0, CAST(N'2025-03-13T18:20:31.967' AS DateTime), N'bb177b0bfd2579eb4356e0a60aa2ec7d.jpg', N'666666789', NULL)
INSERT [dbo].[Usuarios] ([ID_Usuario], [Nombre], [Apellidos], [Email], [Pass_Hash], [Salt], [Pass], [Fecha_Registro], [Activo], [Ultimo_Acceso], [Foto_Perfil], [Telefono], [Token_Recuperacion]) VALUES (2, N'Paco', N'García Serrano', N'paco.garciaserrano@tajamar365.com', 0xFCC8C36BDE529DC3602232706D4C94E95311EB1799289ECE39DBC2F6B720922A, N'¾ÁÙÇEù£×°jsÜe¦®Aõ®\]ÐRåe·ì*_Bèæ9Øì¹t÷¹', N'paco', CAST(N'2025-03-12T19:27:16.633' AS DateTime), 0, CAST(N'2025-03-13T13:48:23.403' AS DateTime), N'2_6823a567-d81b-49c1-b519-739bbe5b5db5.png', NULL, NULL)
INSERT [dbo].[Usuarios] ([ID_Usuario], [Nombre], [Apellidos], [Email], [Pass_Hash], [Salt], [Pass], [Fecha_Registro], [Activo], [Ultimo_Acceso], [Foto_Perfil], [Telefono], [Token_Recuperacion]) VALUES (3, N'Daniel', N'Rodríguez Lancha', N'daniel.rodriguezlancha@tajamar365.com', 0x198F52E220578BBB5EC34BC54BF5CB87D252DE69FC057A1FFD410EB4FB3C3D85, N'9Á²¯]·¡$k"Á$»a¬ýÕ0¢3ÓûScN@KÎÆu¤Â:B¦u$å', N'lucialamejor', CAST(N'2025-03-12T19:27:32.267' AS DateTime), 0, CAST(N'2025-03-12T22:45:46.630' AS DateTime), N'3_a62dc911-ec93-4b9f-9742-109d0ba14715.png', NULL, NULL)
INSERT [dbo].[Usuarios] ([ID_Usuario], [Nombre], [Apellidos], [Email], [Pass_Hash], [Salt], [Pass], [Fecha_Registro], [Activo], [Ultimo_Acceso], [Foto_Perfil], [Telefono], [Token_Recuperacion]) VALUES (4, N'Maria Luisa', N'Fernández Montero', N'marialuisa.fernandezmontero@tajamar365.com', 0x4D4DD1125452605C76B0764EF255517D1E5876F65DD322E91A5F46C8D0E27832, N'P?üË$ú.N
ïûTnð
WVg>¯XþeÍÅFäý(H hèXn*N', N'marilusa', CAST(N'2025-03-12T19:27:57.040' AS DateTime), 0, CAST(N'2025-03-13T00:38:54.423' AS DateTime), N'4_104856f6-1ba9-44b6-bd5e-a06543c6ac5c.png', NULL, NULL)
INSERT [dbo].[Usuarios] ([ID_Usuario], [Nombre], [Apellidos], [Email], [Pass_Hash], [Salt], [Pass], [Fecha_Registro], [Activo], [Ultimo_Acceso], [Foto_Perfil], [Telefono], [Token_Recuperacion]) VALUES (5, N'Alfredo', N'Gutiérrez Parra', N'alfredo.gutierrezparra@tajamar365.com', 0xD14C33B6A6717E5354E57D8FF4FD3F4C779F125D9530E4B5C3DDB0E7020D25EA, N'9º MÌSAUíË?^RÍQ[.-Bca(:GYÎR+?ç¥M/Ê3jC I', N'alfredito', CAST(N'2025-03-12T19:28:08.260' AS DateTime), 0, NULL, N'5_eaa4ee3e-38aa-428a-a9ef-a47650c4a189.png', NULL, NULL)
GO
INSERT [dbo].[Usuarios_Roles] ([ID_Usuario], [ID_Rol]) VALUES (1, 3)
INSERT [dbo].[Usuarios_Roles] ([ID_Usuario], [ID_Rol]) VALUES (2, 1)
INSERT [dbo].[Usuarios_Roles] ([ID_Usuario], [ID_Rol]) VALUES (2, 4)
INSERT [dbo].[Usuarios_Roles] ([ID_Usuario], [ID_Rol]) VALUES (3, 2)
INSERT [dbo].[Usuarios_Roles] ([ID_Usuario], [ID_Rol]) VALUES (4, 1)
INSERT [dbo].[Usuarios_Roles] ([ID_Usuario], [ID_Rol]) VALUES (5, 4)
GO
/****** Object:  Index [UQ__Inscripc__76862B1ED5D8FFD1]    Script Date: 14/03/2025 8:50:59 ******/
ALTER TABLE [dbo].[Inscripciones] ADD UNIQUE NONCLUSTERED 
(
	[ID_Estudiante] ASC,
	[ID_Curso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__75E3EFCFDEC26E2E]    Script Date: 14/03/2025 8:50:59 ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuarios__A9D10534ECC545B3]    Script Date: 14/03/2025 8:50:59 ******/
ALTER TABLE [dbo].[Usuarios] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Usuarios_Telefono]    Script Date: 14/03/2025 8:50:59 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_Telefono] ON [dbo].[Usuarios]
(
	[Telefono] ASC
)
WHERE ([Telefono] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comentarios_Calificaciones] ADD  DEFAULT (getdate()) FOR [Fecha_Comentario]
GO
ALTER TABLE [dbo].[Contenidos] ADD  DEFAULT (getdate()) FOR [Fecha_Publicacion]
GO
ALTER TABLE [dbo].[Correcciones] ADD  DEFAULT (getdate()) FOR [Fecha_Correccion]
GO
ALTER TABLE [dbo].[Cursos] ADD  CONSTRAINT [DF__Cursos__Estado__4E88ABD4]  DEFAULT ('Borrador') FOR [Estado]
GO
ALTER TABLE [dbo].[Entregas_Tareas] ADD  DEFAULT (getdate()) FOR [Fecha_Entrega]
GO
ALTER TABLE [dbo].[Entregas_Tareas] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Examenes] ADD  DEFAULT ((1)) FOR [Intentos_Maximos]
GO
ALTER TABLE [dbo].[Examenes] ADD  DEFAULT ((0)) FOR [Penalizacion_Incorrecta]
GO
ALTER TABLE [dbo].[Examenes_Usuarios] ADD  DEFAULT (getdate()) FOR [Fecha_Inicio]
GO
ALTER TABLE [dbo].[Grupos_Chat] ADD  DEFAULT ((0)) FOR [Es_Privado]
GO
ALTER TABLE [dbo].[Grupos_Chat] ADD  DEFAULT (getdate()) FOR [Fecha_Creacion]
GO
ALTER TABLE [dbo].[Historial_Calificaciones] ADD  DEFAULT (getdate()) FOR [Fecha_Calificacion]
GO
ALTER TABLE [dbo].[Inscripciones] ADD  DEFAULT (getdate()) FOR [Fecha_Inscripcion]
GO
ALTER TABLE [dbo].[Inscripciones] ADD  DEFAULT ('Activo') FOR [Estado]
GO
ALTER TABLE [dbo].[Inscripciones] ADD  DEFAULT ('Estudiante') FOR [Rol_Inscrito]
GO
ALTER TABLE [dbo].[Inscripciones] ADD  DEFAULT ((0)) FOR [Porcentaje_Completado]
GO
ALTER TABLE [dbo].[Mensajes_Chat] ADD  DEFAULT (getdate()) FOR [Fecha_Envio]
GO
ALTER TABLE [dbo].[Mensajes_Chat] ADD  DEFAULT ((0)) FOR [Leido]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  DEFAULT ('No_Leida') FOR [Estado]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  DEFAULT (getdate()) FOR [Fecha_Envio]
GO
ALTER TABLE [dbo].[Preguntas] ADD  DEFAULT ((1)) FOR [Valor_Pregunta]
GO
ALTER TABLE [dbo].[Preguntas] ADD  DEFAULT ((0)) FOR [Es_Multiple]
GO
ALTER TABLE [dbo].[Progreso_Inscripciones] ADD  DEFAULT ((0)) FOR [Completo]
GO
ALTER TABLE [dbo].[Respuestas] ADD  DEFAULT ((0)) FOR [Es_Correcta]
GO
ALTER TABLE [dbo].[Respuestas_Desarrollo] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Respuestas_Desarrollo] ADD  DEFAULT (getdate()) FOR [Fecha_Entrega]
GO
ALTER TABLE [dbo].[Respuestas_Usuarios] ADD  DEFAULT (getdate()) FOR [Fecha_Respuesta]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [Fecha_Registro]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Asignaturas]  WITH CHECK ADD  CONSTRAINT [FK__Asignatur__ID_Cu__5441852A] FOREIGN KEY([ID_Curso])
REFERENCES [dbo].[Cursos] ([ID_Curso])
GO
ALTER TABLE [dbo].[Asignaturas] CHECK CONSTRAINT [FK__Asignatur__ID_Cu__5441852A]
GO
ALTER TABLE [dbo].[Comentarios_Calificaciones]  WITH CHECK ADD FOREIGN KEY([ID_Autor])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Comentarios_Calificaciones]  WITH CHECK ADD FOREIGN KEY([ID_Calificacion])
REFERENCES [dbo].[Historial_Calificaciones] ([ID_Calificacion])
GO
ALTER TABLE [dbo].[Contenidos]  WITH CHECK ADD FOREIGN KEY([ID_Tema])
REFERENCES [dbo].[Temas] ([ID_Tema])
GO
ALTER TABLE [dbo].[Correcciones]  WITH CHECK ADD FOREIGN KEY([ID_Profesor])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Correcciones]  WITH CHECK ADD FOREIGN KEY([ID_Respuesta_Desarrollo])
REFERENCES [dbo].[Respuestas_Desarrollo] ([ID_Respuesta_Desarrollo])
GO
ALTER TABLE [dbo].[Cursos]  WITH CHECK ADD  CONSTRAINT [FK__Cursos__ID_Profe__5070F446] FOREIGN KEY([ID_Profesor])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Cursos] CHECK CONSTRAINT [FK__Cursos__ID_Profe__5070F446]
GO
ALTER TABLE [dbo].[Cursos]  WITH CHECK ADD  CONSTRAINT [FK__Cursos__ID_Profe__5165187F] FOREIGN KEY([ID_Profesor_Suplente])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Cursos] CHECK CONSTRAINT [FK__Cursos__ID_Profe__5165187F]
GO
ALTER TABLE [dbo].[Entregas_Tareas]  WITH CHECK ADD FOREIGN KEY([ID_Contenido])
REFERENCES [dbo].[Contenidos] ([ID_Contenido])
GO
ALTER TABLE [dbo].[Entregas_Tareas]  WITH CHECK ADD FOREIGN KEY([ID_Estudiante])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Examenes]  WITH CHECK ADD FOREIGN KEY([ID_Contenido])
REFERENCES [dbo].[Contenidos] ([ID_Contenido])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Examenes_Usuarios]  WITH CHECK ADD FOREIGN KEY([ID_Contenido])
REFERENCES [dbo].[Contenidos] ([ID_Contenido])
GO
ALTER TABLE [dbo].[Examenes_Usuarios]  WITH CHECK ADD FOREIGN KEY([ID_Estudiante])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Historial_Calificaciones]  WITH CHECK ADD FOREIGN KEY([ID_Contenido])
REFERENCES [dbo].[Contenidos] ([ID_Contenido])
GO
ALTER TABLE [dbo].[Historial_Calificaciones]  WITH CHECK ADD FOREIGN KEY([ID_Estudiante])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Historial_Calificaciones]  WITH CHECK ADD FOREIGN KEY([ID_Profesor_Calificador])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Inscripciones]  WITH CHECK ADD  CONSTRAINT [FK__Inscripci__ID_Cu__04E4BC85] FOREIGN KEY([ID_Curso])
REFERENCES [dbo].[Cursos] ([ID_Curso])
GO
ALTER TABLE [dbo].[Inscripciones] CHECK CONSTRAINT [FK__Inscripci__ID_Cu__04E4BC85]
GO
ALTER TABLE [dbo].[Inscripciones]  WITH CHECK ADD FOREIGN KEY([ID_Estudiante])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Mensajes_Chat]  WITH CHECK ADD FOREIGN KEY([ID_Emisor])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Mensajes_Chat]  WITH CHECK ADD FOREIGN KEY([ID_Grupo])
REFERENCES [dbo].[Grupos_Chat] ([ID_Grupo])
GO
ALTER TABLE [dbo].[Miembros_Grupo]  WITH CHECK ADD FOREIGN KEY([ID_Grupo])
REFERENCES [dbo].[Grupos_Chat] ([ID_Grupo])
GO
ALTER TABLE [dbo].[Miembros_Grupo]  WITH CHECK ADD FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD FOREIGN KEY([Enviado_Por])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Preguntas]  WITH CHECK ADD FOREIGN KEY([ID_Contenido])
REFERENCES [dbo].[Contenidos] ([ID_Contenido])
GO
ALTER TABLE [dbo].[Profesores_Asignaturas]  WITH CHECK ADD FOREIGN KEY([ID_Asignatura])
REFERENCES [dbo].[Asignaturas] ([ID_Asignatura])
GO
ALTER TABLE [dbo].[Profesores_Asignaturas]  WITH CHECK ADD FOREIGN KEY([ID_Profesor])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Progreso_Inscripciones]  WITH CHECK ADD FOREIGN KEY([ID_Contenido])
REFERENCES [dbo].[Contenidos] ([ID_Contenido])
GO
ALTER TABLE [dbo].[Progreso_Inscripciones]  WITH CHECK ADD FOREIGN KEY([ID_Inscripcion])
REFERENCES [dbo].[Inscripciones] ([ID_Inscripcion])
GO
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD FOREIGN KEY([ID_Pregunta])
REFERENCES [dbo].[Preguntas] ([ID_Pregunta])
GO
ALTER TABLE [dbo].[Respuestas_Desarrollo]  WITH CHECK ADD FOREIGN KEY([ID_Estudiante])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Respuestas_Desarrollo]  WITH CHECK ADD FOREIGN KEY([ID_Intento])
REFERENCES [dbo].[Examenes_Usuarios] ([ID_Intento])
GO
ALTER TABLE [dbo].[Respuestas_Desarrollo]  WITH CHECK ADD FOREIGN KEY([ID_Pregunta])
REFERENCES [dbo].[Preguntas] ([ID_Pregunta])
GO
ALTER TABLE [dbo].[Respuestas_Usuarios]  WITH CHECK ADD FOREIGN KEY([ID_Estudiante])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Respuestas_Usuarios]  WITH CHECK ADD FOREIGN KEY([ID_Intento])
REFERENCES [dbo].[Examenes_Usuarios] ([ID_Intento])
GO
ALTER TABLE [dbo].[Respuestas_Usuarios]  WITH CHECK ADD FOREIGN KEY([ID_Pregunta])
REFERENCES [dbo].[Preguntas] ([ID_Pregunta])
GO
ALTER TABLE [dbo].[Respuestas_Usuarios]  WITH CHECK ADD FOREIGN KEY([ID_Respuesta])
REFERENCES [dbo].[Respuestas] ([ID_Respuesta])
GO
ALTER TABLE [dbo].[Temas]  WITH CHECK ADD FOREIGN KEY([ID_Asignatura])
REFERENCES [dbo].[Asignaturas] ([ID_Asignatura])
GO
ALTER TABLE [dbo].[Usuarios_Roles]  WITH CHECK ADD FOREIGN KEY([ID_Rol])
REFERENCES [dbo].[Roles] ([ID_Rol])
GO
ALTER TABLE [dbo].[Usuarios_Roles]  WITH CHECK ADD FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuarios] ([ID_Usuario])
GO
ALTER TABLE [dbo].[Contenidos]  WITH CHECK ADD CHECK  (([Tipo]='Examen' OR [Tipo]='Quiz' OR [Tipo]='Tarea' OR [Tipo]='Enlace' OR [Tipo]='Documento' OR [Tipo]='Video'))
GO
ALTER TABLE [dbo].[Cursos]  WITH CHECK ADD  CONSTRAINT [CK__Cursos__Estado__4F7CD00D] CHECK  (([Estado]='Suspendido' OR [Estado]='Archivado' OR [Estado]='Finalizado' OR [Estado]='Activo' OR [Estado]='Borrador'))
GO
ALTER TABLE [dbo].[Cursos] CHECK CONSTRAINT [CK__Cursos__Estado__4F7CD00D]
GO
ALTER TABLE [dbo].[Entregas_Tareas]  WITH CHECK ADD CHECK  (([Estado]='Revisado' OR [Estado]='Pendiente'))
GO
ALTER TABLE [dbo].[Inscripciones]  WITH CHECK ADD CHECK  (([Estado]='Abandonado' OR [Estado]='Completado' OR [Estado]='Activo'))
GO
ALTER TABLE [dbo].[Inscripciones]  WITH CHECK ADD CHECK  (([Rol_Inscrito]='Invitado' OR [Rol_Inscrito]='Tutor' OR [Rol_Inscrito]='Estudiante'))
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD CHECK  (([Estado]='Leida' OR [Estado]='No_Leida'))
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD CHECK  (([Tipo]='Calificacion' OR [Tipo]='Foro' OR [Tipo]='Tarea' OR [Tipo]='Curso' OR [Tipo]='Sistema'))
GO
ALTER TABLE [dbo].[Preguntas]  WITH CHECK ADD CHECK  (([Tipo]='Desarrollo' OR [Tipo]='Opcion_Multiple'))
GO
ALTER TABLE [dbo].[Respuestas_Desarrollo]  WITH CHECK ADD CHECK  (([Estado]='Revisado' OR [Estado]='Pendiente'))
GO

