DECLARE @UserId INT;
SELECT @UserId = UserId FROM Users WHERE Email = 'arko2207053@stud.kuet.ac.bd';

IF @UserId IS NOT NULL
BEGIN
    -- Seed Article
    IF NOT EXISTS (SELECT 1 FROM Articles WHERE AuthorId = @UserId)
    BEGIN
        INSERT INTO Articles (Title, Content, AuthorId, PublishDate, ThumbnailUrl)
        VALUES ('Introduction to Neural Networks', 'Neural networks form the base of deep learning. This article explains the perceptron model, activation functions, and backpropagation in a beginner-friendly manner.

We will cover the basics of weights, biases, and how the network learns from data using gradient descent.', @UserId, GETDATE(), 'assets/images/article-preview.png');
    END

    -- Seed Research
    IF NOT EXISTS (SELECT 1 FROM Research WHERE AuthorId = @UserId)
    BEGIN
        INSERT INTO Research (Title, Abstract, AuthorId, PublishDate, DownloadLink, Domain)
        VALUES ('Transformer Models in Bengali NLP', 'This paper investigates the performance of BERT-based models when fine-tuned on Bengali text datasets. We observe significant improvements in text classification and named entity recognition compared to traditional recurrent models.', @UserId, GETDATE(), '#', 'NLP');
    END

    -- Seed Dataset
    IF NOT EXISTS (SELECT 1 FROM Datasets WHERE UploaderId = @UserId)
    BEGIN
        INSERT INTO Datasets (Title, Description, UploaderId, UploadDate, DownloadLink, Size, Domain)
        VALUES ('KUET Traffic Image Dataset', 'A collection of over 10,000 annotated images of traffic conditions around KUET campus. Useful for training vehicle detection and traffic density estimation models.', @UserId, GETDATE(), '#', '2.5 GB', 'Computer Vision');
    END
END

-- Seed Event (Events don't have an AuthorId in the schema, just global)
IF NOT EXISTS (SELECT 1 FROM Events WHERE Title = 'KMinds Intro Datathon')
BEGIN
    INSERT INTO Events (Title, Description, EventDate, Location, ImageUrl)
    VALUES ('KMinds Intro Datathon', 'A beginner-friendly datathon for new recruits. Form a team, analyze the provided dataset, and submit your predictions on Kaggle. Great prizes to be won!', DATEADD(day, 14, GETDATE()), 'KUET CSE Seminar Room', 'assets/images/event-datathon.png');
END
