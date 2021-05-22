variable "AWS_DEFAULT_REGION" {
  
}

variable "AWS_SECRET_ACCESS_KEY" {
  
}

variable "AWS_ACCESS_KEY_ID" {
  
}

variable "stage" {
    default = "dev"
}

variable "code_version" {
  default = "0.1"
}



variable "signature_certificate" {}
variable "signature_certiticate_password" {}
variable "default_api_key" {
  default = "53c1136e-81a9-4dd1-a6f9-6759ea53ad9a"
}
