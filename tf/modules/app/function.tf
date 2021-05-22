locals{
  lambda_function_name = "Digital_Signature_Service_API"
  source_code_hash = filemd5("modules/s3/function.zip")
}
resource "aws_cloudwatch_log_group" "function_log" {
  name              = "/aws/lambda/${local.lambda_function_name}"
  retention_in_days = 14
}

resource "aws_lambda_function" "digital_signature_api" {
   function_name = local.lambda_function_name

   s3_bucket = var.deploy_bucket
   s3_key    = "${var.code_version}/function.zip"

   # "main" is the filename within the zip file (main.js) and "handler"
   # is the name of the property under which the handler function was
   # exported in that file.
   handler = "DigitalSignatureApi::DigitalSignatureApi.Function::Get"
   runtime = "dotnetcore3.1"
   # handler= "index.handler"
   # runtime = "nodejs10.x"
   timeout = 30
   role = aws_iam_role.lambda_exec.arn

   depends_on  =[
     aws_cloudwatch_log_group.function_log,
    aws_iam_role_policy_attachment.lambda_logs,
   ]
   source_code_hash = local.source_code_hash
   environment {
    variables = {
      SOURCE_CODE_HASH = local.source_code_hash
      Certificate__Certificate = var.signature_certificate
      Certificate__Password = var.signature_certiticate_password
    }
  }
}

data "aws_iam_policy_document" "lambda-role-policy" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }
  }
}

 # IAM role which dictates what other AWS services the Lambda function
 # may access.
resource "aws_iam_role" "lambda_exec" {
   name = "Digital-Signature-Sign-Lambda-Role"
   assume_role_policy = data.aws_iam_policy_document.lambda-role-policy.json
}


resource "aws_iam_policy" "lambda_logging" {
  name        = "lambda_logging"
  path        = "/"
  description = "IAM policy for logging from a lambda"

  policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:*",
      "Effect": "Allow"
    }
  ]
}
EOF
}

resource "aws_iam_role_policy_attachment" "lambda_logs" {
  role       = aws_iam_role.lambda_exec.name
  policy_arn = aws_iam_policy.lambda_logging.arn
}
