resource "aws_api_gateway_rest_api" "apiLambda" {
  name        = "Digital Signature Sign Service"
}
resource "aws_api_gateway_resource" "proxy" {
   rest_api_id = aws_api_gateway_rest_api.apiLambda.id
   parent_id   = aws_api_gateway_rest_api.apiLambda.root_resource_id
   path_part   = "{proxy+}"

}

resource "aws_api_gateway_api_key" "gateway-api-key" {
  name = "default"
}

resource "aws_api_gateway_method" "proxyMethod" {
   rest_api_id   = aws_api_gateway_rest_api.apiLambda.id
   resource_id   = aws_api_gateway_resource.proxy.id
   http_method   = "ANY"
   authorization = "NONE"
   
}

resource "aws_api_gateway_integration" "lambda1" {
   rest_api_id = aws_api_gateway_rest_api.apiLambda.id
   resource_id = aws_api_gateway_method.proxyMethod.resource_id
   http_method = aws_api_gateway_method.proxyMethod.http_method

   integration_http_method = "POST"
   type                    = "AWS_PROXY"
   uri                     = aws_lambda_function.digital_signature_api.invoke_arn
}

resource "aws_api_gateway_method" "proxy_root" {
   rest_api_id   = aws_api_gateway_rest_api.apiLambda.id
   resource_id   = aws_api_gateway_rest_api.apiLambda.root_resource_id
   http_method   = "ANY"
   authorization = "NONE"
   api_key_required = true
}

resource "aws_api_gateway_integration" "api_root" {
   rest_api_id = aws_api_gateway_rest_api.apiLambda.id
   resource_id = aws_api_gateway_method.proxy_root.resource_id
   http_method = aws_api_gateway_method.proxy_root.http_method

   integration_http_method = "POST"
   type                    = "AWS_PROXY"
   uri                     = aws_lambda_function.digital_signature_api.invoke_arn
}


resource "aws_api_gateway_deployment" "api" {
   depends_on = [
     aws_api_gateway_integration.lambda1,
     aws_api_gateway_integration.api_root,
   ]

   rest_api_id = aws_api_gateway_rest_api.apiLambda.id
   //stage_name  = var.stage
}

resource "aws_cloudwatch_log_group" "gateway_api_logs" {
  name              = "API-Gateway-Execution-Logs_${aws_api_gateway_rest_api.apiLambda.id}/${var.stage}"
  retention_in_days = 7
}



resource "aws_lambda_permission" "apigw1" {
   statement_id  = "AllowAPIGatewayInvoke"
   action        = "lambda:InvokeFunction"
   function_name = aws_lambda_function.digital_signature_api.function_name
   principal     = "apigateway.amazonaws.com"

   # The "/*/*" portion grants access from any method on any resource
   # within the API Gateway REST API.
   source_arn = "${aws_api_gateway_rest_api.apiLambda.execution_arn}/*/*"
}


resource "aws_api_gateway_stage" "development" {
  deployment_id = aws_api_gateway_deployment.api.id
  rest_api_id   = aws_api_gateway_rest_api.apiLambda.id
  stage_name    = var.stage
}

resource "aws_api_gateway_usage_plan" "sign_api_plan" {
  name         = "Digital Signature Default Plan"
  description  = "The default API usage plan"
  product_code = "MYCODE"

  api_stages {
    api_id = aws_api_gateway_rest_api.apiLambda.id
    stage  = aws_api_gateway_stage.development.stage_name
  }

  quota_settings {
    limit  = 20
    offset = 2
    period = "WEEK"
  }

  throttle_settings {
    burst_limit = 5
    rate_limit  = 10
  }
}


resource "aws_api_gateway_usage_plan_key" "main" {
  key_id        = aws_api_gateway_api_key.gateway-api-key.id
  key_type      = "API_KEY"
  usage_plan_id = aws_api_gateway_usage_plan.sign_api_plan.id
}
