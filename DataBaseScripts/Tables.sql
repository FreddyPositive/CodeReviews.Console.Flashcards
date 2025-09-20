

-- Switch to the database
USE FLASH_CARD_LEARNING_DB;
GO

-- ========================
-- Table: Stacks
-- ========================
CREATE TABLE Stacks (
    id INT IDENTITY(1,1) PRIMARY KEY,
    stack_name NVARCHAR(100) NOT NULL,
    created_date DATETIME NOT NULL DEFAULT GETDATE()
);

-- ========================
-- Table: FlashCards
-- ========================
CREATE TABLE FlashCards (
    id INT IDENTITY(1,1) PRIMARY KEY,
    question NVARCHAR(300) NOT NULL,
    answer NVARCHAR(300) NOT NULL,
    stack_id INT NOT NULL,
    created_date DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_FlashCards_Stacks FOREIGN KEY (stack_id) REFERENCES Stacks(id)
);

-- ========================
-- Table: LearningLog
-- ========================
CREATE TABLE LearningLog (
    id INT IDENTITY(1,1) PRIMARY KEY,
    flash_cards_reviewed NVARCHAR(max) NOT NULL,
    stack_id INT NOT NULL,
    session_date DATETIME NOT NULL DEFAULT GETDATE(),
    duration_minutes INT NOT NULL, -- store as minutes (e.g., 150 = 2.5 hours)
    CONSTRAINT FK_LearningLog_Stacks FOREIGN KEY (stack_id) REFERENCES Stacks(id)
);

