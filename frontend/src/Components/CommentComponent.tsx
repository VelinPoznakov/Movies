import { Card, Flex, Typography } from "antd";
import type { CommentResponse } from "../types/commentType";

const { Text } = Typography;

function CommentComponent({ comment }: { comment: CommentResponse }) {
  const createdDate = new Date(comment.createdAt).toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "2-digit"
  });

  return (
    <Card
      size="small"
      hoverable
      variant="borderless"
      style={{ width: "100%" }}
      bodyStyle={{ padding: "12px 16px" }}
    >
      <Flex justify="space-between" align="center" style={{ marginBottom: 8 }}>
        <Text type="secondary" style={{ fontSize: 12 }}>
          {createdDate}
        </Text>
        <Text strong style={{ fontSize: 12 }}>
          {comment.username ?? "Unknown"}
        </Text>
      </Flex>

      <Text>{comment.content}</Text>
    </Card>
  );
}

export default CommentComponent;
