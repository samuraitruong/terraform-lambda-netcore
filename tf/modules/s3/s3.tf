resource "aws_s3_bucket" "deploy" {
  bucket = "digital-signature-service-deploy"
  acl    = "private"

  tags = {
    Name        = "The bucket to upload zip file"
    Environment = "Dev"
  }
}


resource "aws_s3_bucket_object" "default_version" {
  bucket = aws_s3_bucket.deploy.bucket
  key    = "0.1/function.zip"
  source = "${path.module}/function.zip"

  etag = filemd5("${path.module}/function.zip")
}
