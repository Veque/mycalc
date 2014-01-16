USE [MyCalc2]
GO

/****** Object:  StoredProcedure [dbo].[SaveCombinationEnergy]    Script Date: 10/12/13 20:00:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SaveCombinationEnergy]
	@c1 int,
	@c2 int,
	@c3 int,
	@c4 int,
	@c5 int,
	@c6 int,
	@c7 int,
	@e5 smallint,
	@e7 smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		insert into dbo.Energy(
		c1,
		c2,
		c3,
		c4,
		c5,
		c6,
		c7,
		e5,
		e7 )
		values (
		@c1,
		@c2,
		@c3,
		@c4,
		@c5,
		@c6,
		@c7,
		@e5,
		@e7 )

END

GO

