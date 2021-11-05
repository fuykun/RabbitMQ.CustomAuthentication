FROM rabbitmq:3.9-management
WORKDIR /

RUN rabbitmq-plugins enable rabbitmq_management_agent
RUN rabbitmq-plugins enable rabbitmq_management
RUN rabbitmq-plugins enable rabbitmq_auth_backend_http


COPY rabbitmq.conf /etc/rabbitmq/

RUN chown rabbitmq:rabbitmq /etc/rabbitmq/rabbitmq.conf

ENTRYPOINT ["rabbitmq-server"]